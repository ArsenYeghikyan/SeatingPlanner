using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EventSeatingPlanner.App.Services;

namespace EventSeatingPlanner.App;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped<AuthState>();
        builder.Services.AddScoped<AuthMessageHandler>();
        builder.Services.AddScoped(sp =>
        {
            var handler = sp.GetRequiredService<AuthMessageHandler>();
            handler.InnerHandler = new HttpClientHandler();
            return new HttpClient(handler) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
        });
        builder.Services.AddScoped<EventSeatingApiClient>();
        builder.Services.AddScoped<AuthApiClient>();

        await builder.Build().RunAsync();
    }
}
