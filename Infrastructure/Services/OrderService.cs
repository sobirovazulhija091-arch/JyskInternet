using System.Net;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Responses;
using Domain.DTOs;

public class OrderService(ApplicationDbContext dbContext):IOrderService
{
     private readonly ApplicationDbContext context = dbContext;

    public async Task<Response<string>> DeleteOrderAsync(int id)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
    if (order == null)
        {
          return new Response<string>(HttpStatusCode.NotFound, "Order not found");   
        }
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
        }
        await context.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "Order created successfully");
    }

    public async Task<PagedResult<Order>> GetAllAsync(FilterOrder filter, PagedQuery query)
    {
         var orders = context.Orders.AsQueryable();

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
        var orders = await context.Orders.Include(o=>o.User).Where(o => o.UserId == userId).ToListAsync();
        return new Response<List<Order>>(HttpStatusCode.OK, "Success", orders);
    }

    public async Task<Response<string>> UpdateStatusAsync(int id, EnumStatus status)
    {
       var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
    if (order == null)
        {
         return new Response<string>(HttpStatusCode.NotFound, "Order not found");   
        }
    order.Status = status;
    await context.SaveChangesAsync();
    return new Response<string>(HttpStatusCode.OK, "Order status updated");
    }
}
