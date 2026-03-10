using System.Net;
using System.Net.Http.Json;
using WebJysk.Admin.Models;

namespace WebJysk.Admin.Services;

public class ProductApiService
{
    private readonly ApiClient _apiClient;

    public ProductApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    private static async Task<Response<string>?> EnsureSuccessAndReadAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<Response<string>>();

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException("Session expired or insufficient permissions. Please log in with an admin account.");

        var body = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Request failed ({(int)response.StatusCode}): {body}");
    }

    public async Task<PagedResult<Product>?> GetAllAsync(FilterProduct? filter, PagedQuery query)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var queryParams = new List<string>
        {
            $"page={query.Page}", $"pageSize={query.PageSize}"
        };
        if (filter?.Name != null) queryParams.Add($"name={Uri.EscapeDataString(filter.Name)}");
        if (filter?.CategoryId != null) queryParams.Add($"categoryId={filter.CategoryId}");
        if (filter?.BrandId != null) queryParams.Add($"brandId={filter.BrandId}");
        if (filter?.MinPrice != null) queryParams.Add($"minPrice={filter.MinPrice}");
        if (filter?.MaxPrice != null) queryParams.Add($"maxPrice={filter.MaxPrice}");
        if (filter?.IsActive != null) queryParams.Add($"isActive={filter.IsActive}");
        var url = "api/product?" + string.Join("&", queryParams);
        return await client.GetFromJsonAsync<PagedResult<Product>>(url);
    }

    public async Task<Response<Product>?> GetByIdAsync(int id)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        return await client.GetFromJsonAsync<Response<Product>>($"api/product/{id}");
    }

    public async Task<Response<string>?> AddAsync(ProductDto dto)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var response = await client.PostAsJsonAsync("api/product", dto);
        return await EnsureSuccessAndReadAsync(response);
    }

    public async Task<Response<string>?> UpdateAsync(int id, ProductDto dto)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var response = await client.PutAsJsonAsync($"api/product/{id}", dto);
        return await EnsureSuccessAndReadAsync(response);
    }

    public async Task<Response<string>?> DeleteAsync(int id)
    {
        var client = await _apiClient.GetAuthorizedClientAsync();
        var response = await client.DeleteAsync($"api/product/{id}");
        return await EnsureSuccessAndReadAsync(response);
    }
}
