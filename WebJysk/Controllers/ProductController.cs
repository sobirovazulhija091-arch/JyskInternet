using System.Threading.Tasks;
using Domain.DTOs;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService service) : ControllerBase
{
    private readonly IProductService _service=service;

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<Response<string>> AddAsync([FromBody] ProductDto dto)
    {
        return await _service.AddAsync(dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/image")]
    public async Task<Response<string>> UploadImageAsync(int id, IFormFile file)
    {
        return await _service.UploadImageAsync(id, file);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<Response<string>> UpdateAsync(int id, [FromBody] UpdateProductDto dto)
    {
        return await _service.UpdateAsync(id, dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<Response<string>> DeleteAsync(int id)
    {
        return await _service.DeleteAsync(id);
    }
   
    [HttpGet]
    public async Task<PagedResult<Product>> GetAllAsync(
        [FromQuery] FilterProduct filter,
        [FromQuery] PagedQuery query)
    {
        return await _service.GetAllAsync(filter, query);
    }

    [HttpGet("{id}")]
    public async Task<Response<Product>> GetByIdAsync(int id)
    {
        return await _service.GetByIdAsync(id);
    }
  
    [HttpGet("category/{categoryId}")]
    public async Task<Response<List<Product>>> GetByCategoryAsync(int categoryId)
    {
        return await _service.GetByCategoryAsync(categoryId);
    }
    [HttpGet("brand/{brandId}")]
    public async Task<Response<List<Product>>> GetByBrandAsync(int brandId)
    {
        return await _service.GetByBrandAsync(brandId);
    }
    
    [HttpGet("filter")]
    public async Task<Response<List<Product>>> FilterAsync(decimal? minPrice,decimal? maxPrice,int? brandId)
    {
        return await _service.FilterAsync(minPrice, maxPrice, brandId);
    }
    [HttpGet("search")]
    public async Task<Response<List<Product>>> SearchAsync(string keyword)
    {
        return await _service.SearchAsync(keyword);
    }

    [HttpGet("sort")]
    public async Task<Response<List<Product>>> SortByPriceAsync(bool ascending)
    {
        return await _service.SortByPriceAsync(ascending);
    }

    [HttpGet("top-selling")]
    public async Task<Response<List<Product>>> GetTopSellingAsync()
    {
        return await _service.GetTopSellingAsync();
    }
    [HttpGet("{id}/reviews")]
    public async Task<Response<Product>> GetWithReviewsAsync(int id)
    {
        return await _service.GetWithReviewsAsync(id);
    }
}
