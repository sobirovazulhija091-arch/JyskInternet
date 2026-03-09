using System.Net.Http.Json;
using WebJysk.Admin.Models;

namespace WebJysk.Admin.Services;

public class UserApiService
{
    private readonly ApiClient _apiClient;

    public UserApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResult<UserModel>?> GetAllAsync(FilterUser? filter, PagedQuery query)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var queryParams = new List<string> { $"page={query.Page}", $"pageSize={query.PageSize}" };
        if (filter?.Email != null) queryParams.Add($"email={Uri.EscapeDataString(filter.Email)}");
        if (filter?.FullName != null) queryParams.Add($"fullName={Uri.EscapeDataString(filter.FullName)}");
        var url = "api/user?" + string.Join("&", queryParams);
        return await client.GetFromJsonAsync<PagedResult<UserModel>>(url);
    }

    public async Task<Response<UserModel>?> GetByIdAsync(string id)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        return await client.GetFromJsonAsync<Response<UserModel>>($"api/user/{id}");
    }

    public async Task<Response<string>?> DeleteAsync(string id)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var response = await client.DeleteAsync($"api/user/{id}");
        return await response.Content.ReadFromJsonAsync<Response<string>>();
    }
}
