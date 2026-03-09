using System.Net.Http.Json;
using WebJysk.Marketplace.Models;

namespace WebJysk.Marketplace.Services;

public class ProductApiService
{
    private readonly HttpClient _http;

    public ProductApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PagedResult<Product>?> GetProductsAsync(int page = 1, int pageSize = 12, int? categoryId = null, int? brandId = null, string? search = null)
    {
        var query = $"page={page}&pageSize={pageSize}&isActive=true";
        if (categoryId.HasValue) query += $"&categoryId={categoryId}";
        if (brandId.HasValue) query += $"&brandId={brandId}";
        if (!string.IsNullOrWhiteSpace(search)) query += $"&name={Uri.EscapeDataString(search)}";
        return await _http.GetFromJsonAsync<PagedResult<Product>>($"api/product?{query}");
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var res = await _http.GetFromJsonAsync<Response<Product>>($"api/product/{id}");
        return res?.Data;
    }

    public async Task<List<Product>?> SearchAsync(string keyword)
    {
        var res = await _http.GetFromJsonAsync<Response<List<Product>>>($"api/product/search?keyword={Uri.EscapeDataString(keyword)}");
        return res?.Data;
    }

    public async Task<List<Product>?> GetByCategoryAsync(int categoryId)
    {
        var res = await _http.GetFromJsonAsync<Response<List<Product>>>($"api/product/category/{categoryId}");
        return res?.Data;
    }

    public async Task<List<Product>?> GetTopSellingAsync()
    {
        var res = await _http.GetFromJsonAsync<Response<List<Product>>>("api/product/top-selling");
        return res?.Data;
    }
}
