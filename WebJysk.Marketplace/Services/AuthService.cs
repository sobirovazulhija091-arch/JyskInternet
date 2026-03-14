using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.JSInterop;

namespace WebJysk.Marketplace.Services;

public class AuthService
{
    private const string TokenKey = "authToken";
    private const string GuestKey = "authGuest";
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public event Action? OnAuthChanged;

    public AuthService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(string email, string password, string fullName, string? phone = null)
    {
        try
        {
            var payload = new { email, password, fullName, phone };
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var jsonContent = new StringContent(JsonSerializer.Serialize(payload, options), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/auth/register", jsonContent);
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
                return (false, "Registration failed. Email may already exist.");
            }

            var tokenDoc = JsonDocument.Parse(json);
            if (tokenDoc.RootElement.TryGetProperty("token", out var tokenEl))
            {
                var token = tokenEl.GetString();
                if (!string.IsNullOrEmpty(token))
                {
                    await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
                    await _js.InvokeVoidAsync("localStorage.removeItem", GuestKey);
                    OnAuthChanged?.Invoke();
                    return (true, null);
                }
            }
            return (false, "Invalid response.");
        }
        catch (HttpRequestException)
        {
            return (false, "Cannot connect to server.");
        }
    }

    public async Task<(bool Success, string? Error)> LoginAsync(string email, string password)
    {
        try
        {
            var payload = new { email, password };
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
                return (false, "Invalid email or password.");
            }

            var tokenDoc = JsonDocument.Parse(json);
            if (tokenDoc.RootElement.TryGetProperty("token", out var tokenEl))
            {
                var token = tokenEl.GetString();
                if (!string.IsNullOrEmpty(token))
                {
                    await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
                    await _js.InvokeVoidAsync("localStorage.removeItem", GuestKey);
                    OnAuthChanged?.Invoke();
                    return (true, null);
                }
            }
            return (false, "Invalid response from server.");
        }
        catch (HttpRequestException)
        {
            return (false, "Cannot connect to server.");
        }
    }

    public async Task SetGuestAsync()
    {
        await _js.InvokeVoidAsync("localStorage.setItem", GuestKey, "1");
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        OnAuthChanged?.Invoke();
    }

    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        await _js.InvokeVoidAsync("localStorage.removeItem", GuestKey);
        OnAuthChanged?.Invoke();
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
    }

    public async Task<bool> IsGuestAsync()
    {
        var guest = await _js.InvokeAsync<string?>("localStorage.getItem", GuestKey);
        return guest == "1";
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }

    public async Task<bool> IsAdminAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return false;

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/auth/me");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode) return false;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("claims", out var claimsEl)) return false;

            foreach (var c in claimsEl.EnumerateArray())
            {
                if (c.TryGetProperty("type", out var typeEl))
                {
                    var type = typeEl.GetString();
                    if ((type?.Contains("role", StringComparison.OrdinalIgnoreCase) == true ||
                         type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role") &&
                        c.TryGetProperty("value", out var valueEl) &&
                        valueEl.GetString()?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        return true;
                    }
                }
            }
        }
        catch { }

        return false;
    }

    public async Task<(string? Name, string? Email, string? Phone)?> GetUserInfoAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return null;

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/auth/me");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            string? name = null;
            string? email = null;
            string? phone = null;

            if (doc.RootElement.TryGetProperty("name", out var nameEl))
                name = nameEl.GetString();
            if (doc.RootElement.TryGetProperty("email", out var emailEl))
                email = emailEl.GetString();
            if (doc.RootElement.TryGetProperty("phone", out var phoneEl))
                phone = phoneEl.GetString();

            if (doc.RootElement.TryGetProperty("claims", out var claimsEl))
            {
                foreach (var c in claimsEl.EnumerateArray())
                {
                    if (c.TryGetProperty("type", out var t) && c.TryGetProperty("value", out var v))
                    {
                        var type = t.GetString();
                        var val = v.GetString();
                        if (type?.Contains("email", StringComparison.OrdinalIgnoreCase) == true && string.IsNullOrEmpty(email)) email = val;
                        if (type?.Contains("name", StringComparison.OrdinalIgnoreCase) == true && !string.IsNullOrEmpty(val) && string.IsNullOrEmpty(name)) name = val;
                    }
                }
            }
            return (name, email, phone);
        }
        catch { }
        return null;
    }

    public async Task<bool> HasChosenAsync()
    {
        var token = await GetTokenAsync();
        var guest = await _js.InvokeAsync<string?>("localStorage.getItem", GuestKey);
        return !string.IsNullOrEmpty(token) || guest == "1";
    }
}
