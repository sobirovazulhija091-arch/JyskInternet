using System.Net;
using System.Net.Http.Json;
using WebJysk.Marketplace.Models;

namespace WebJysk.Marketplace.Services;

public class OrderApiService
{
    private readonly ApiClient _apiClient;

    public OrderApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<(bool Success, string? Error)> CreateOrderAsync(CreateOrderDto dto)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var response = await client.PostAsJsonAsync("api/order/create", dto);

        if (response.IsSuccessStatusCode)
            return (true, null);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return (false, "Please log in to place an order.");

        var body = await response.Content.ReadAsStringAsync();
        return (false, body.Length > 200 ? "Order failed. Please try again." : body);
    }
}
