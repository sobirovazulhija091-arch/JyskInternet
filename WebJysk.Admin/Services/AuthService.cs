using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.JSInterop;
using WebJysk.Admin.Models;

namespace WebJysk.Admin.Services;

public class AuthService
{
    private const string TokenKey = "authToken";
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public AuthService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<(bool Success, string? Error)> LoginAsync(string email, string password)
    {
        try
        {
            var payload = new LoginDto { Email = email, Password = password };
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var jsonContent = new StringContent(JsonSerializer.Serialize(payload, options), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/auth/login", jsonContent);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("message", out var msg))
                        return (false, msg.GetString());
                }
                catch { }
                return (false, response.StatusCode switch
                {
                    System.Net.HttpStatusCode.BadRequest => "Invalid request. Check your email and password.",
                    System.Net.HttpStatusCode.Unauthorized => "Invalid email or password.",
                    _ => "Login failed. Please try again."
                });
            }

            var tokenDoc = JsonDocument.Parse(json);
            if (tokenDoc.RootElement.TryGetProperty("token", out var tokenEl))
            {
                var token = tokenEl.GetString();
                if (!string.IsNullOrEmpty(token))
                {
                    await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
                    return (true, null);
                }
            }
            return (false, "Invalid response from server.");
        }
        catch (HttpRequestException)
        {
            return (false, "Cannot connect to server. Ensure the API is running.");
        }
        catch (TaskCanceledException)
        {
            return (false, "Request timed out.");
        }
    }

    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
}
