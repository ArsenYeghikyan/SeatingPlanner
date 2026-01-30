using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using EventSeatingPlanner.App.Models;
using Microsoft.JSInterop;

namespace EventSeatingPlanner.App.Services;

public sealed class AuthService
{
    private const string TokenStorageKey = "esp.jwt.token";
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private string? _token;
    private string? _email;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_token);
    public string? Token => _token;
    public string? Email => _email;

    public async Task InitializeAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenStorageKey);
        if (!string.IsNullOrWhiteSpace(token))
        {
            _token = token;
            _email = ExtractEmail(token);
            SetAuthorizationHeader(token);
        }
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Не удалось зарегистрироваться.");

        await StoreTokenAsync(authResponse.Token);
        _email = authResponse.Email;
        return authResponse;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Не удалось войти.");

        await StoreTokenAsync(authResponse.Token);
        _email = authResponse.Email;
        return authResponse;
    }

    public async Task LogoutAsync()
    {
        _token = null;
        _email = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenStorageKey);
    }

    private async Task StoreTokenAsync(string token)
    {
        _token = token;
        SetAuthorizationHeader(token);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenStorageKey, token);
    }

    private void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private static string? ExtractEmail(string jwtToken)
    {
        try
        {
            var parts = jwtToken.Split('.');
            if (parts.Length != 3)
            {
                return null;
            }

            var payload = parts[1];
            var jsonBytes = Base64UrlDecode(payload);
            var payloadJson = JsonDocument.Parse(jsonBytes);

            if (payloadJson.RootElement.TryGetProperty("email", out var emailElement))
            {
                return emailElement.GetString();
            }
        }
        catch
        {
            return null;
        }

        return null;
    }

    private static byte[] Base64UrlDecode(string input)
    {
        var padded = input.Replace('-', '+').Replace('_', '/');
        switch (padded.Length % 4)
        {
            case 2:
                padded += "==";
                break;
            case 3:
                padded += "=";
                break;
        }

        return Convert.FromBase64String(padded);
    }
}
