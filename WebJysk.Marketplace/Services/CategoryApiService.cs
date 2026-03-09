using System.Net.Http.Json;
using WebJysk.Marketplace.Models;

namespace WebJysk.Marketplace.Services;

public class CategoryApiService
{
    private readonly HttpClient _http;

    public CategoryApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Category>?> GetAllAsync()
    {
        var res = await _http.GetFromJsonAsync<Response<List<Category>>>("api/category");
        return res?.Data;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        var res = await _http.GetFromJsonAsync<Response<Category>>($"api/category/{id}");
        return res?.Data;
    }
}
