using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComponentHub.Client.Components;
using ComponentHub.Client.Components.Features.Auth;
using ComponentHub.Client.Components.Features.RedirectHelper;
using ComponentHub.Shared;
using ComponentHub.Shared.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using Serilog;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<RedirectHelper>();
builder.Services.AddScoped(sp =>
{
    return new HttpClient()
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    };
});

builder.Services.AddValidatorsFromAssemblies(new[]
{
    Assembly.GetAssembly(typeof(Program)),
    Assembly.GetAssembly(typeof(ComponentHubContext))
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Debug()
    .WriteTo.BrowserConsole()
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));




builder.Services.AddScoped<AuthApiClient>();
builder.Services.AddScoped<IdentityAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    s => s.GetRequiredService<IdentityAuthenticationStateProvider>());

await builder.Build().RunAsync();