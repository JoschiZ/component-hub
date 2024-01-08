using System.Reflection;
using System.Text.Json.Serialization;
using ComponentHub.DB;
using ComponentHub.Server.Features.Authentication;
using ComponentHub.Server.Helper;
using FastEndpoints.ClientGen.Kiota;
using FastEndpoints.Swagger;
using FluentValidation;
using Kiota.Builder;
using Microsoft.IdentityModel.Protocols.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// This loads configs that may be needed for later registrations so it needs to be first!
builder.AddEnvToConfig();

builder.AddAuthentication();
builder.Services.AddAntiforgery();
builder.Services.AddEfCore(optionsBuilder =>
{
#if DEBUG
    optionsBuilder.EnableSensitiveDataLogging();
#endif
});

builder.Services.AddFastEndpoints(options =>
    {
        options.IncludeAbstractValidators = true;
    })
    .SwaggerDocument(options =>
    {
        options.DocumentSettings = settings =>
        {
            settings.DocumentName = "v1";
        };
        options.ShortSchemaNames = true;
        
        
        options.SerializerSettings = serializerOptions =>
        {
            serializerOptions.Converters.Add(new JsonStringEnumConverter());
        };
    })
    .AddOutputCache();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Debug()
    .WriteTo.Console()
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

// Gets all AbstractValidator<T> implementations and registers them from this and the shared assembly
builder.Services.AddValidatorsFromAssemblies(new[]
{
    Assembly.GetAssembly(typeof(Program))
}, ServiceLifetime.Singleton, includeInternalTypes: true);



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
    config.Errors.UseProblemDetails();
    config.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
})
    .UseSwaggerGen()
    .UseOutputCache();

if (app.Environment.IsDevelopment())
{
    var clientPath = app.Configuration.GetValue<string>("ClientPath");
    if (clientPath is null)
    {
        throw new InvalidConfigurationException("clientPath was null");
    }

    var workingDirectory = Environment.CurrentDirectory;
    var outputPath = Directory.GetParent(workingDirectory) + "/" + Path.Combine("ComponentHub.Client", "ApiClients", "CSharp");
    Console.WriteLine("Outputting Kiota Client To: " + outputPath);
    //spits out generated client files to disk if app is run with '--generateclients true' commandline argument
    await app.GenerateApiClientsAndExitAsync(
        c =>
        {
            c.SwaggerDocumentName = "v1";
            c.Language = GenerationLanguage.CSharp;
            c.OutputPath = outputPath;
            c.ClientNamespaceName = "ComponentHub.ApiClients";
            c.ClientClassName = "ComponentHubBaseClient";
        });
}


app.Run();


public partial class Program { }