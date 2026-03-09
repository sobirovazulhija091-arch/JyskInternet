using System.Threading.Tasks;
using Domain.DTOs;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService service) : ControllerBase
{
    private readonly IOrderService _service=service;
   
    [HttpPost("create")]
    public async Task<Response<string>> CreateOrderAsync(OrderDto dto)
    {
        dto.UserId = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return await _service.CreateOrderAsync(dto);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<PagedResult<Order>> GetAllAsync(
        [FromQuery] FilterOrder filter,
        [FromQuery] PagedQuery query)
    {
        return await _service.GetAllAsync(filter, query);
    }

    [Authorize(Roles = "User")]
    [HttpGet("my-orders")]
    public async Task<Response<List<Order>>> GetUserOrdersAsync()
    {
        var userId = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new Response<List<Order>>(System.Net.HttpStatusCode.Unauthorized, "Unauthorized");
        }
        return await _service.GetUserOrdersAsync(userId);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet("{id}")]
    public async Task<Response<Order>> GetByIdAsync(int id)
    {
        return await _service.GetByIdAsync(id);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/status")]
    public async Task<Response<string>> UpdateStatusAsync(int id, [FromBody] UpdateOrderStatusDto dto)
    {
        return await _service.UpdateStatusAsync(id, dto.Status);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<Response<string>> DeleteOrderAsync(int id)
    {
        return await _service.DeleteOrderAsync(id);
    }
}
