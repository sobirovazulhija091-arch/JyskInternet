using System.Net;
using System.Net.Http.Json;
using WebJysk.Marketplace.Models;

namespace WebJysk.Marketplace.Services;

public class ReviewApiService
{
    private readonly ApiClient _apiClient;

    public ReviewApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<(bool Success, string? Error)> AddReviewAsync(int productId, int rating, string comment)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var dto = new { ProductId = productId, Rating = rating, Comment = comment };
        var response = await client.PostAsJsonAsync("api/review", dto);

        if (response.IsSuccessStatusCode)
            return (true, null);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return (false, "Please log in to add a review.");

        var body = await response.Content.ReadAsStringAsync();
        return (false, body.Length > 150 ? "Failed to add review." : body);
    }

    public async Task<List<ReviewModel>?> GetProductReviewsAsync(int productId)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var res = await client.GetFromJsonAsync<Response<List<ReviewModel>>>($"api/review/product/{productId}");
        return res?.Data;
    }
}
