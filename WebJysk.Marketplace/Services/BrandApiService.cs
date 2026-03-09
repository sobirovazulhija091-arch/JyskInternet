using System.Net.Http.Json;
using WebJysk.Marketplace.Models;

namespace WebJysk.Marketplace.Services;

public class BrandApiService
{
    private readonly HttpClient _http;

    public BrandApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Brand>?> GetAllAsync()
    {
        var res = await _http.GetFromJsonAsync<Response<List<Brand>>>("api/brand");
        return res?.Data;
    }
}
