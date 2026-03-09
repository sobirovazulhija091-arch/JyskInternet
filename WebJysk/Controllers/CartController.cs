using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class CartController(ICartService cartService) : ControllerBase
{
    private readonly ICartService service = cartService;
    private string? GetUserId() => User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<Response<string>> AddItemAsync(int productId, int quantity)
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new Response<string>(System.Net.HttpStatusCode.Unauthorized, "Unauthorized");
        }
        return await service.AddItemAsync(userId, productId, quantity);
    }
    [Authorize(Roles = "User")]
    [HttpPut]
    public async Task<Response<string>> UpdateQuantityAsync(int productId, int quantity)
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new Response<string>(System.Net.HttpStatusCode.Unauthorized, "Unauthorized");
        }
        return await service.UpdateQuantityAsync(userId, productId, quantity);
    }
    [Authorize(Roles = "User")]
    [HttpDelete("remove")]
    public async Task<Response<string>> RemoveItemAsync(int productId)
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new Response<string>(System.Net.HttpStatusCode.Unauthorized, "Unauthorized");
        }
        return await service.RemoveItemAsync(userId, productId);
    }
    [Authorize(Roles = "User")]
    [HttpDelete]
    public async Task<Response<string>> DeltetCartAsync()
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new Response<string>(System.Net.HttpStatusCode.Unauthorized, "Unauthorized");
        }
        return await service.DeltetCartAsync(userId);
    }
    [Authorize(Roles = "User")]
    [HttpGet]
 public async Task<Response<Cart>> GetUserCartAsync()
{
    var userId = GetUserId();

    if (string.IsNullOrWhiteSpace(userId))
    {
        return new Response<Cart>(System.Net.HttpStatusCode.Unauthorized, "Unauthorized");
    }

    var cartResponse = await service.GetUserCartAsync(userId);

    if (cartResponse.Data == null)
    {
        cartResponse.Data = new Cart 
        { 
            UserId = userId, 
            CartItems = new List<CartItem>() 
        };
    }

    return cartResponse;
}
}
