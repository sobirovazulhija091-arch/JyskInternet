
using Microsoft.EntityFrameworkCore;
using System.Net;
public class DeliveryService(ApplicationDbContext dbContext):IDeliveryService
{
      private readonly ApplicationDbContext context = dbContext;

    public async Task<Response<string>> CreateDeliveryAsync(int orderId, EnumDeliveryType type)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o=>o.Id==orderId);
       if (order == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Order not found");
        }

        var existDeliver = await context.Deliveries.FirstOrDefaultAsync(o=>o.OrderId==orderId);
           if (existDeliver != null)
           {
        return new Response<string>(HttpStatusCode.BadRequest, "Delivery already exists");
          };
         var delivery = new Delivery
       {
        OrderId = orderId,
        DeliveryType = type,
        DeliveryStatus = EnumDeliveryStatus.Preparing,
        DeliveredAt = DateTime.UtcNow
       };
        await context.Deliveries.AddAsync(delivery);
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK,"Deliver Type add");
    }

    public async Task<Response<Delivery>> GetByOrderIdAsync(int orderId)
    {
       var get = await context.Deliveries.Include(o=>o.Order).FirstOrDefaultAsync(o=>o.OrderId==orderId);
       if (get == null)
       {
        return new Response<Delivery>(HttpStatusCode.NotFound,"Delivery not found");
       }
      return new Response<Delivery>(HttpStatusCode.OK,"Ok,",get);
    }

    public async Task<Response<string>> UpdateStatusAsync(int orderId, EnumDeliveryStatus status)
    {
        var delivery = await context.Deliveries.FirstOrDefaultAsync(d => d.OrderId == orderId);
        if (delivery == null)
        {
          return new Response<string>(HttpStatusCode.NotFound, "Delivery not found");  
        } 
        delivery.DeliveryStatus = status;
        if (status == EnumDeliveryStatus.Delivered)
        {
            delivery.DeliveredAt = DateTime.UtcNow;
        }
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK,"Updated successfully");  

    }
}
