using System.Net.Http.Json;
using System.Text.Json;

namespace WebJysk.Marketplace.Services;

public class AuthApiService
{
    private readonly HttpClient _http;

    public AuthApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var response = await _http.PostAsJsonAsync("api/email/forgot-password", new { Email = email });
        return response.IsSuccessStatusCode;
    }

    public async Task<(bool Success, string? Error)> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var dto = new { Email = email, Token = token, NewPassword = newPassword };
        var response = await _http.PostAsJsonAsync("api/email/reset-password", dto);

        if (response.IsSuccessStatusCode)
            return (true, null);

        var body = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(body)) return (false, "Reset failed. The link may have expired.");
        try
        {
            var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("Description", out var desc))
                return (false, desc.GetRawText());
            if (doc.RootElement.ValueKind == JsonValueKind.String)
                return (false, body.Trim('"'));
        }
        catch { }
        return (false, body.Length > 150 ? "Invalid or expired reset link." : body);
    }
}
