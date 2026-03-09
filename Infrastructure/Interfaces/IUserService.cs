using Domain.DTOs;
using Infrastructure.Responses;

public interface IUserService
{
  Task<PagedResult<User>> GetAllAsync(FilterUser filter,PagedQuery query);
    Task<Response<User>> GetByIdAsync(string id);
    Task<Response<string>> UpdateAsync(string id, UpdateUserDto dto);
    Task<Response<string>> DeleteAsync(string id);
}