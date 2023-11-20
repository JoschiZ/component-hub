using System.Reflection;
using ComponentHub.Server.Auth;
using ComponentHub.Server.Helper;
using ComponentHub.Shared;
using ComponentHub.Shared.Helper.Repositories;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);
// This loads configs that may be needed for later registrations so it needs to be first!
builder.AddEnvToConfig();

builder.AddAuthentication();
builder.Services.AddAntiforgery();
builder.Services.UseRepositories();

builder.Services.AddFastEndpoints(options =>
{
    options.IncludeAbstractValidators = true;
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Debug()
    .WriteTo.Console()
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

// Gets all AbstractValidator<T> implementations and registers them from this and the shared assembly
builder.Services.AddValidatorsFromAssemblies(new[]
{
    Assembly.GetAssembly(typeof(Program)),
    Assembly.GetAssembly(typeof(IUnitOfWork))
}, ServiceLifetime.Singleton, includeInternalTypes: true);
builder.Services.AddFluentValidationAutoValidation();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.MapFallbackToFile("index.html");
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(config =>
{
    config.Endpoints.Configurator = definition =>
    {

    };
});

app.Run();


public partial class Program { }