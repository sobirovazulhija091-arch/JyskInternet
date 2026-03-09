using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography.X509Certificates;

[ApiController]
[Route("api/[controller]")]
public class DescountController(IDiscountService descount):ControllerBase
{
    public readonly IDiscountService service=descount;

   [Authorize(Roles = "Admin")]
   [HttpPost("create")]
   public async Task<Response<string>> CreateAsync(DiscountDto dto)
    {
        return await service.CreateAsync(dto);
    }
    [Authorize(Roles = "User,Admin")]
    [HttpPost("validate")]

    public async Task<Response<bool>> ValidateAsync(string code)
    {
        return await service.ValidateAsync(code);
    }
    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    public async Task<Response<decimal>> ApplyDiscountAsync(int orderId, string code)
    {
        return await service.ApplyDiscountAsync(orderId,code);
    }
}
