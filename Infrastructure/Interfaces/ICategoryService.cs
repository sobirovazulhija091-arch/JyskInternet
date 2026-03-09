using Microsoft.AspNetCore.Http;

public interface ICategoryService
{
    Task<Response<string>> AddAsync(CategoryDto dto);
    Task<Response<string>> UpdateAsync(int id, UpdateCategoryDto dto);
    Task<Response<string>> DeleteAsync(int id);
    Task<Response<string>> UploadImageAsync(int categoryId, IFormFile file);
    Task<Response<List<Category>>> GetAllAsync();
    Task<Response<Category>> GetByIdAsync(int id);
    Task<Response<List<Category>>> GetSubCategoriesAsync(int parentId);
}