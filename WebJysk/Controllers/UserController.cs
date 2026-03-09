using System.Threading.Tasks;
using Domain.DTOs;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService  userService):ControllerBase
{
    private readonly IUserService  service = userService;
     [Authorize(Roles = "Admin")]
     [HttpGet]
    public async Task<PagedResult<User>> GetAllAsync([FromQuery] FilterUser filter,[FromQuery] PagedQuery query)
    {
       return await service.GetAllAsync(filter, query);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<Response<User>> GetByIdAsync(string id)
    {
        return await service.GetByIdAsync(id);   
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<Response<string>> DeleteAsync(string id)
    {
         return await service.DeleteAsync(id);
    }
}
