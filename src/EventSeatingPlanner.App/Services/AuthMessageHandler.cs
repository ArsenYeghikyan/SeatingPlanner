using System.Net.Http.Headers;

namespace EventSeatingPlanner.App.Services;

public sealed class AuthMessageHandler(AuthState authState) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(authState.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authState.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
