using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StoreController(IStoreService service) : ControllerBase
{
    private readonly IStoreService _service = service;

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<Response<string>> AddAsync(StoreDto dto)
    {
        return await _service.AddAsync(dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<Response<string>> UpdateAsync(int id, StoreDto dto)
    {
        return await _service.UpdateAsync(id, new UpdateStoreDto
        {
            Name = dto.Name,
            Address = dto.Address,
            City = dto.City,
            Phone = dto.Phone,
            WorkingHours = dto.WorkingHours
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<Response<string>> DeleteAsync(int id)
    {
        return await _service.DeleteAsync(id);
    }

    [HttpGet]
    public async Task<Response<List<Store>>> GetAllAsync()
    {
        return await _service.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<Response<Store>> GetByIdAsync(int id)
    {
        return await _service.GetByIdAsync(id);
    }
}