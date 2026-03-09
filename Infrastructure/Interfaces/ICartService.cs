public interface ICartService
{
    Task<Response<string>> AddItemAsync(string userId, int productId, int quantity);
    Task<Response<string>> UpdateQuantityAsync(string userId, int productId, int quantity);
    Task<Response<string>> RemoveItemAsync(string userId, int productId);//
    Task<Response<string>> DeltetCartAsync(string userId);
    Task<Response<Cart>> GetUserCartAsync(string userId);
}
// kapzina