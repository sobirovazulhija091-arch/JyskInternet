using System.Net.Http.Json;
using WebJysk.Admin.Models;

namespace WebJysk.Admin.Services;

public class OrderApiService
{
    private readonly ApiClient _apiClient;

    public OrderApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResult<Order>?> GetAllAsync(FilterOrder? filter, PagedQuery query)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var queryParams = new List<string> { $"page={query.Page}", $"pageSize={query.PageSize}" };
        if (filter?.UserId != null) queryParams.Add($"userId={Uri.EscapeDataString(filter.UserId)}");
        if (filter?.Status != null) queryParams.Add($"status={(int)filter.Status}");
        if (filter?.FromDate != null) queryParams.Add($"fromDate={filter.FromDate:O}");
        if (filter?.ToDate != null) queryParams.Add($"toDate={filter.ToDate:O}");
        var url = "api/order?" + string.Join("&", queryParams);
        return await client.GetFromJsonAsync<PagedResult<Order>>(url);
    }

    public async Task<Response<Order>?> GetByIdAsync(int id)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        return await client.GetFromJsonAsync<Response<Order>>($"api/order/{id}");
    }

    public async Task<Response<string>?> UpdateStatusAsync(int id, EnumStatus status)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var body = new { Status = (int)status };
        var response = await client.PutAsJsonAsync($"api/order/{id}/status", body);
        return await response.Content.ReadFromJsonAsync<Response<string>>();
    }
}
