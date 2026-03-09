using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace WebJysk.Admin.Services;

public class ApiClient
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;
    private const string TokenKey = "authToken";

    public ApiClient(HttpClient http, AuthService auth)
    {
        _http = http;
        _auth = auth;
    }

    public async Task<HttpClient> GetAuthorizedClientAsync()
    {
        var token = await _auth.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
        return _http;
    }
}
