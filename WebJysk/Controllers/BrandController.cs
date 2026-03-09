using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class BrandController(IBrandService service) : ControllerBase
{
    private readonly IBrandService _service=service;

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<Response<string>> AddAsync(BrandDto dto)
    {
        var result = await _service.AddAsync(dto);
        return result;
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<Response<string>> UpdateAsync(int id, BrandDto dto)
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
    public async Task<Response<List<Brand>>> GetAllAsync()
    {
         return await _service.GetAllAsync();
    }
    [HttpGet("{id}")]
    public async Task<Response<Brand>> GetByIdAsync(int id)
    {
        return await _service.GetByIdAsync(id);

    }
}
