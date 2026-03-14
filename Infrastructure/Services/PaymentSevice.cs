
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
public class PaymentSevice(ApplicationDbContext dbContext):IPaymentService
{
   private readonly ApplicationDbContext context = dbContext;

    public async Task<Response<string>> CreatePaymentAsync(int orderId, EnumPaymentMethod method)
    {
          var order = await context.Orders.FirstOrDefaultAsync(o=>o.Id==orderId);
       if (order == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Order not found");
        }
          var existpay = await context.Payments.FirstOrDefaultAsync(o=>o.OrderId==orderId);
           if (existpay != null)
           {
        return new Response<string>(HttpStatusCode.BadRequest, "Payment already exists");
        };
        var pay = new Payment
        {
         OrderId = orderId,
        PaymentMethod = method,
       PaymentStatus = EnumPaymentStatus.Pending
        };
          await context.Payments.AddAsync(pay);
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK,"Payment  added");
    }

    public async Task<Response<string>> UpdateStatusAsync(int orderId, EnumPaymentStatus status)
    {
         var pay = await context.Payments.FirstOrDefaultAsync(d => d.OrderId == orderId);
        if (pay == null)
        {
          return new Response<string>(HttpStatusCode.NotFound, "Payment not found");  
        } 
        pay.PaymentStatus = status;
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order != null && status == EnumPaymentStatus.Paid)
        {
            order.Status = EnumStatus.Paid;
        }
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK,"Updated successfully");  

    }
}
