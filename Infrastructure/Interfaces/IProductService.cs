using Microsoft.AspNetCore.Http;
using Domain.DTOs;
using Infrastructure.Responses;

public interface IProductService
{
Task<Response<string>> AddAsync(ProductDto dto);
Task<Response<string>> UpdateAsync(int id, UpdateProductDto dto);
Task<Response<string>> DeleteAsync(int id);
Task<PagedResult<Product>> GetAllAsync(FilterProduct filter,PagedQuery query);
Task<Response<Product>> GetByIdAsync(int id); 
Task<Response<List<Product>>> GetByCategoryAsync(int categoryId); 
Task<Response<List<Product>>> GetByBrandAsync(int brandId);
Task<Response<List<Product>>>  FilterAsync(decimal? minPrice, decimal? maxPrice, int? brandId);
Task<Response<List<Product>>>  SearchAsync(string keyword); 
Task<Response<List<Product>>> SortByPriceAsync(bool ascending);
Task<Response<List<Product>>> GetTopSellingAsync();
Task<Response<Product>> GetWithReviewsAsync(int id); 
    Task<Response<string>> UploadImageAsync(int productId, IFormFile file);

}