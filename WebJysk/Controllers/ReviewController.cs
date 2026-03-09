using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class ReviewController(IReviewService service) : ControllerBase
{
    private readonly IReviewService _service=service;
    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<Response<string>> AddReviewAsync(ReviewDto dto)
    {
       dto.UserId = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
       return await _service.AddReviewAsync(dto);
    }
  [HttpGet("product/{productId}")]
    public async Task<Response<List<Review>>> GetProductReviewsAsync(int productId)
    {
        return await _service.GetProductReviewsAsync(productId);
    }
 [HttpGet("product/{productId}/average")]
    public async Task<Response<double>> GetAverageAsync(int productId)
    {
        return await _service.GetAverageRatingAsync(productId);
    }
}
