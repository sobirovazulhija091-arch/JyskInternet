using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService service) : ControllerBase
{
    private readonly ICategoryService _service = service;

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<Response<string>> AddAsync([FromBody] CategoryDto dto)
    {
        return await _service.AddAsync(dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<Response<string>> UpdateAsync(int id, [FromBody] UpdateCategoryDto dto)
    {
        return await _service.UpdateAsync(id, dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<Response<string>> DeleteAsync(int id)
    {
        return await _service.DeleteAsync(id);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/image")]
    public async Task<Response<string>> UploadImageAsync(int id, IFormFile file)
    {
        return await _service.UploadImageAsync(id, file);
    }

    [HttpGet]
    public async Task<Response<List<Category>>> GetAll()
    {
        return await _service.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<Response<Category>> GetById(int id)
    {
        return await _service.GetByIdAsync(id);
    }

    [HttpGet("sub/{parentId}")]
    public async Task<Response<List<Category>>> GetSubCategories(int parentId)
    {
        return await _service.GetSubCategoriesAsync(parentId);
    }
}
