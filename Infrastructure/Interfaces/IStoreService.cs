public interface IStoreService
{
    Task<Response<string>> AddAsync(StoreDto dto);
    Task<Response<string>> UpdateAsync(int id, UpdateStoreDto dto);
    Task<Response<string>> DeleteAsync(int id);
    Task<Response<List<Store>>> GetAllAsync();
    Task<Response<Store>> GetByIdAsync(int id);
}