public interface IDiscountService
{
    Task<Response<string>> CreateAsync(DiscountDto dto);
    Task<Response<bool>> ValidateAsync(string code);
    Task<Response<decimal>> ApplyDiscountAsync(int orderId, string code);
}