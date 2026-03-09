using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography.X509Certificates;

[ApiController]
[Route("api/[controller]")]
public class DeliverController(IDeliveryService deliver):ControllerBase
{
    private readonly IDeliveryService service = deliver;
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<Response<string>> CreateDeliveryAsync(int orderId, EnumDeliveryType type)
    {
        return await service.CreateDeliveryAsync(orderId,type);
    }
    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<Response<string>> UpdateStatusAsync(int orderId, EnumDeliveryStatus status)
    {
        return await service.UpdateStatusAsync(orderId,status);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<Response<Delivery>> GetByOrderIdAsync(int orderId)
    {
         return await service.GetByOrderIdAsync(orderId);
    }
}
