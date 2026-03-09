public interface IBrandService
{
    Task<Response<string>> AddAsync(BrandDto dto);
    Task<Response<string>> UpdateAsync(int id, BrandDto dto);
    Task<Response<string>> DeleteAsync(int id);

    Task<Response<List<Brand>>> GetAllAsync();
    Task<Response<Brand>> GetByIdAsync(int id);
}