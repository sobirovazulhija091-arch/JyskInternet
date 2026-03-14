using System.Net.Http.Json;
using WebJysk.Admin.Models;

namespace WebJysk.Admin.Services;

public class ReviewApiService
{
    private readonly ApiClient _apiClient;

    public ReviewApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResult<ReviewModel>?> GetAllAsync(FilterReview? filter, PagedQuery query)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var queryParams = new List<string> { $"page={query.Page}", $"pageSize={query.PageSize}" };
        if (filter?.ProductId != null) queryParams.Add($"productId={filter.ProductId}");
        if (filter?.UserId != null) queryParams.Add($"userId={Uri.EscapeDataString(filter.UserId)}");
        var url = "api/review?" + string.Join("&", queryParams);
        return await client.GetFromJsonAsync<PagedResult<ReviewModel>>(url);
    }
}
