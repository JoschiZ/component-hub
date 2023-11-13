using System.Reflection;
using ComponentHub.Server.Auth;
using ComponentHub.Server.Helper;
using ComponentHub.Shared.Auth;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);
// This loads configs that may be needed for later registrations so it needs to be first!
builder.AddEnvToConfig();

builder.AddAuthentication();

builder.Services
    .AddDbContext<ComponentHubContext>(optionsBuilder =>
        optionsBuilder.UseSqlite("Filename=app.db").UseOpenIddict());


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Gets all AbstractValidator<T> implementations and registers them from this and the shared assembly
builder.Services.AddValidatorsFromAssemblies(new[]
{
    Assembly.GetAssembly(typeof(Program)),
    Assembly.GetAssembly(typeof(RegisterOptions))
});
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

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseAuthentication();
app.UseAuthorization();

app.Run();