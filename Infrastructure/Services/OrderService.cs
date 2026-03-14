using System.Net;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Responses;
using Domain.DTOs;
using Infrastructure.Data;

public class OrderService(ApplicationDbContext dbContext, IEmailService emailService) : IOrderService
{
    private readonly ApplicationDbContext context = dbContext;
    private readonly IEmailService _email = emailService;

    public async Task<Response<string>> DeleteOrderAsync(int id)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
            return new Response<string>(HttpStatusCode.NotFound, "Order not found");

        await RestoreStockAsync(order);
        context.Orders.Remove(order);
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "Order deleted successfully");
    }

    public async Task<Response<string>> CreateOrderAsync(OrderDto dto)
    {
        if (dto.TotalAmount <= 0)
            return new Response<string>(HttpStatusCode.BadRequest, "TotalAmount must be greater than 0");
        if (string.IsNullOrWhiteSpace(dto.DeliveryAddress))
            return new Response<string>(HttpStatusCode.BadRequest, "Delivery address is required");
        if (string.IsNullOrWhiteSpace(dto.UserId))
            return new Response<string>(HttpStatusCode.Unauthorized, "User must be logged in");
        if (dto.Items == null || !dto.Items.Any())
            return new Response<string>(HttpStatusCode.BadRequest, "Order must contain at least one item");

        foreach (var item in dto.Items)
        {
            if (item.Quantity <= 0)
                return new Response<string>(HttpStatusCode.BadRequest, $"Invalid quantity for product {item.ProductId}");
        }

        var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p);

        foreach (var item in dto.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
                return new Response<string>(HttpStatusCode.BadRequest, $"Product {item.ProductId} not found");
            if (product.StockQuantity < item.Quantity)
                return new Response<string>(HttpStatusCode.BadRequest,
                    $"Insufficient stock for '{product.Name}'. Available: {product.StockQuantity}, requested: {item.Quantity}");
        }

        var order = new Order
        {
            UserId = dto.UserId,
            PaymentMethod = dto.PaymentMethod,
            Status = dto.status,
            TotalAmount = dto.TotalAmount,
            DeliveryAddress = dto.DeliveryAddress,
            OrderDate = DateTime.UtcNow
        };
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        foreach (var item in dto.Items)
        {
            context.OrderItems.Add(new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            });
            var product = products[item.ProductId];
            product.StockQuantity -= item.Quantity;
        }
        await context.SaveChangesAsync();

        await SendOrderConfirmationAsync(order.Id);
        return new Response<string>(HttpStatusCode.OK, "Order created successfully");
    }

    private async Task SendOrderConfirmationAsync(int orderId)
    {
        try
        {
            var order = await context.Orders.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order?.User?.Email == null) return;

            var body = $@"Hello {order.User.FullName},

Thank you for your order #{orderId}!

Your order total: {order.TotalAmount:N2}
Delivery address: {order.DeliveryAddress}

Our courier will deliver your order. You will receive an email when your order is on the way and when it has been delivered.

- JYSK";
            await _email.SendAsync(order.User.Email, $"Order Confirmed #{orderId} - JYSK", body);
        }
        catch { /* Don't fail order if email fails */ }
    }

    public async Task<PagedResult<Order>> GetAllAsync(FilterOrder filter, PagedQuery query)
    {
         var orders = context.Orders.Include(o => o.User).AsQueryable();

        if (filter.UserId!=null)
        {
            orders = orders.Where(o => o.UserId == filter.UserId);
        }
        if (filter.Status!=null)
        {
            orders = orders.Where(o => o.Status == filter.Status);
         }
        if (filter.FromDate.HasValue)
        {
            orders = orders.Where(o => o.OrderDate >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            orders = orders.Where(o => o.OrderDate <= filter.ToDate.Value);
        } 
        if (filter.MinTotalAmount.HasValue)
        {
            orders = orders.Where(o => o.TotalAmount >= filter.MinTotalAmount.Value);
        }
        if (filter.MaxTotalAmount.HasValue)
        {
            orders = orders.Where(o => o.TotalAmount <= filter.MaxTotalAmount.Value);
        }
          var total = await orders.CountAsync();
      var page = query.Page > 0 ? query.Page : 1;
      var pageSize = query.PageSize > 0 ? query.PageSize : 10;
      orders = orders.Skip((page-1)*pageSize).Take(pageSize);
    var users = await orders.ToListAsync();

    return new PagedResult<Order>
    {
        Items = users,
        Page = page,
        PageSize = pageSize,
        TotalCount = total,
        TotalPages = (int)Math.Ceiling((double)total / pageSize)
    };
    }

    public async Task<Response<Order>> GetByIdAsync(int id)
    {
        var order = await context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);

    if (order == null)
        {
          return new Response<Order>(HttpStatusCode.NotFound, "Order not found");   
        }
    return new Response<Order>(HttpStatusCode.OK, "Success", order);
    }

    public  async Task<Response<List<Order>>> GetUserOrdersAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new Response<List<Order>>(HttpStatusCode.Unauthorized, "Unauthorized");
        }
        var orders = await context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems).ThenInclude(i => i.Product)
            .Where(o => o.UserId == userId)
            .ToListAsync();
        return new Response<List<Order>>(HttpStatusCode.OK, "Success", orders);
    }

    public async Task<Response<string>> UpdateStatusAsync(int id, EnumStatus status)
    {
        var order = await context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
            return new Response<string>(HttpStatusCode.NotFound, "Order not found");

        var previousStatus = order.Status;
        order.Status = status;

        if (status == EnumStatus.Cancelled && previousStatus != EnumStatus.Cancelled)
            await RestoreStockAsync(order);

        await context.SaveChangesAsync();

        await SendDeliveryStatusEmailAsync(order, status);
        return new Response<string>(HttpStatusCode.OK, "Order status updated");
    }

    private async Task RestoreStockAsync(Order order)
    {
        foreach (var item in order.OrderItems ?? [])
        {
            if (item.Product != null)
            {
                item.Product.StockQuantity += item.Quantity;
            }
            else
            {
                var product = await context.Products.FindAsync(item.ProductId);
                if (product != null)
                    product.StockQuantity += item.Quantity;
            }
        }
    }

    private async Task SendDeliveryStatusEmailAsync(Order order, EnumStatus status)
    {
        try
        {
            if (string.IsNullOrEmpty(order.User?.Email)) return;

            string subject, body;
            switch (status)
            {
                case EnumStatus.Processing:
                    subject = $"Order #{order.Id} - On the way";
                    body = $@"Hello {order.User.FullName},

Your order #{order.Id} has been taken by our courier and is being prepared for delivery.

Delivery address: {order.DeliveryAddress}

You will receive another email when your order has been delivered.

- JYSK";
                    break;
                case EnumStatus.Delivered:
                    subject = $"Order #{order.Id} - Delivered";
                    body = $@"Hello {order.User.FullName},

Your order #{order.Id} has been delivered successfully!

Thank you for shopping with JYSK.

- JYSK";
                    break;
                default:
                    return;
            }

            await _email.SendAsync(order.User.Email, subject, body);
        }
        catch { /* Don't fail status update if email fails */ }
    }
}
