using System.Net.Http.Json;
using WebJysk.Admin.Models;

namespace WebJysk.Admin.Services;

public class CategoryApiService
{
    private readonly ApiClient _apiClient;

    public CategoryApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Response<List<Category>>?> GetAllAsync()
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        return await client.GetFromJsonAsync<Response<List<Category>>>("api/category");
    }

    public async Task<Response<Category>?> GetByIdAsync(int id)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        return await client.GetFromJsonAsync<Response<Category>>($"api/category/{id}");
    }

    public async Task<Response<string>?> AddAsync(CategoryDto dto)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var payload = new { dto.Name, ParentId = dto.ParentId == 0 ? null : dto.ParentId, dto.ImageUrl };
        var response = await client.PostAsJsonAsync("api/category", payload);
        return await response.Content.ReadFromJsonAsync<Response<string>>();
    }

    public async Task<Response<string>?> UpdateAsync(int id, CategoryDto dto)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var payload = new { dto.Name, ParentId = dto.ParentId == 0 ? null : dto.ParentId, dto.ImageUrl };
        var response = await client.PutAsJsonAsync($"api/category/{id}", payload);
        return await response.Content.ReadFromJsonAsync<Response<string>>();
    }

    public async Task<Response<string>?> DeleteAsync(int id)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var response = await client.DeleteAsync($"api/category/{id}");
        return await response.Content.ReadFromJsonAsync<Response<string>>();
    }
}
