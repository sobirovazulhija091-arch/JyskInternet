using Domain.DTOs;
using Infrastructure.Responses;

public interface IOrderService
{
    Task<Response<string>> CreateOrderAsync(OrderDto dto);
   Task<PagedResult<Order>> GetAllAsync(FilterOrder filter,PagedQuery query);
    Task<Response<List<Order>>> GetUserOrdersAsync(string userId);
    Task<Response<Order>> GetByIdAsync(int id);
    Task<Response<string>> UpdateStatusAsync(int id, EnumStatus status);
    Task<Response<string>> DeleteOrderAsync(int id);
}