namespace EventSeatingPlanner.App.Services;

public sealed class AuthState
{
    public string? Token { get; private set; }
    public string? Email { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Token);

    public void Set(string token, string email)
    {
        Token = token;
        Email = email;
    }

    public void Clear()
    {
        Token = null;
        Email = null;
    }
}
