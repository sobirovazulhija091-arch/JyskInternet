public interface IWarehouseService
{
    Task<Response<string>> CreateAsync(WarehouseDto dto);
    Task<Response<string>> UpdateAsync(int id, UpdateWarehouseDto dto);
    Task<Response<string>> DeleteAsync(int id);
    Task<Response<List<Warehouse>>> GetAllAsync();
    Task<Response<Warehouse>> GetByIdAsync(int id);
    Task<Response<string>>  AddStockAsync(AddStockDto dto);
    Task<Response<string>> DecreaseStockAsync(DecrStockDto dto);
    Task<Response<int>> CheckStockAsync(int productId);//
}
