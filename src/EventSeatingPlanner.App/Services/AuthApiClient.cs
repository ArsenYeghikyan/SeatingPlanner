using System.Net.Http.Json;
using EventSeatingPlanner.App.Models;

namespace EventSeatingPlanner.App.Services;

public sealed class AuthApiClient(HttpClient httpClient, AuthState authState)
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/register", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Не удалось зарегистрироваться.");
        authState.Set(auth.Token, auth.Email);
        return auth;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/login", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Не удалось войти.");
        authState.Set(auth.Token, auth.Email);
        return auth;
    }

    public void Logout()
    {
        authState.Clear();
    }
}
