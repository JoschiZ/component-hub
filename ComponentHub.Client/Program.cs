using System.Reflection;
using ComponentHub.Client;
using ComponentHub.Client.ApiClients;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComponentHub.Client.Components;
using ComponentHub.Client.Components.Features.Auth;
using ComponentHub.Client.Components.Features.Components;
using ComponentHub.Client.Components.Helper;
using ComponentHub.Client.Core;
using ComponentHub.Domain.Api;
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


//builder.Services.AddHttpClient("ApiClient", client => { client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); }).AddHttpMessageHandler<ValidationDelegatingHandler>().AddHttpMessageHandler<RedirectDelegatingHandler>();

builder.Services.AddHttpClient("ApiClient")
    .AddTypedClient<ComponentHubClient>(client => new ComponentHubClient(client))
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<ErrorDelegatingHandler>()
    .AddHttpMessageHandler<ValidationDelegatingHandler>()
    .AddHttpMessageHandler<RedirectDelegatingHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));
builder.Services.AddSingleton<SnackbarHelperService>();
builder.Services.AddSingleton<ValidationDelegatingHandler>();
builder.Services.AddSingleton<ErrorDelegatingHandler>();
builder.Services.AddScoped<RedirectDelegatingHandler>();


builder.Services.AddValidatorsFromAssemblies(new[]
{
    Assembly.GetAssembly(typeof(Program)),
    Assembly.GetAssembly(typeof(Endpoints))
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Debug()
    .WriteTo.BrowserConsole()
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
builder.Services.AddScoped<Test>();

builder.Services.AddScoped<ComponentService>();

builder.Services.AddScoped<AuthApiClient>();
builder.Services.AddScoped<IdentityAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    s => s.GetRequiredService<IdentityAuthenticationStateProvider>());

await builder.Build().RunAsync();