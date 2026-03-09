using System.Net.Http.Json;
using WebJysk.Admin.Models;

namespace WebJysk.Admin.Services;

public class BrandApiService
{
    private readonly ApiClient _apiClient;

    public BrandApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Response<List<Brand>>?> GetAllAsync()
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        return await client.GetFromJsonAsync<Response<List<Brand>>>("api/brand");
    }

    public async Task<Response<Brand>?> GetByIdAsync(int id)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        return await client.GetFromJsonAsync<Response<Brand>>($"api/brand/{id}");
    }

    public async Task<Response<string>?> AddAsync(BrandDto dto)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var response = await client.PostAsJsonAsync("api/brand", dto);
        return await response.Content.ReadFromJsonAsync<Response<string>>();
    }

    public async Task<Response<string>?> UpdateAsync(int id, BrandDto dto)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var response = await client.PutAsJsonAsync($"api/brand/{id}", dto);
        return await response.Content.ReadFromJsonAsync<Response<string>>();
    }

    public async Task<Response<string>?> DeleteAsync(int id)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var response = await client.DeleteAsync($"api/brand/{id}");
        return await response.Content.ReadFromJsonAsync<Response<string>>();
    }
}
