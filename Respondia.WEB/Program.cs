using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Respondia.Core.Entities;
using Respondia.DataAccess.Data;
using Respondia.WEB.Components;
using Respondia.WEB.Components.Account;
using Serilog;
using Respondia.Core.Middlewares;
using Respondia.Core.Utils;
using Respondia.BL.Utils;
using Respondia.Core.Extensions;
using System.Reflection;
using Microsoft.Extensions.Caching.Hybrid;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;
using Respondia.BL.Services.EmailServices;
using Respondia.DataAccess.Utils;
using Respondia.WEB.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Only for Development
builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = true;
});

// Appsettings.json config
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

// Serilog config
builder.Host.UseSerilogLogging(builder.Configuration);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Identity Services
builder.Services.AddIdentityServices(builder.Configuration);

// DbContext 
builder.Services.AddAppDbContext(builder.Configuration);

// Automatic DI register
builder.Services.AddInjectables(
    Assembly.GetExecutingAssembly(),
    typeof(EntryPointCore).Assembly,
    typeof(EntryPointBusinessLogic).Assembly,
    typeof(EntryPointDataAccess).Assembly
);

// EmailServices
builder.Services.AddEmailServices(builder.Configuration);

// HttpContextAccesor
builder.Services.AddHttpContextAccessor();

// FluentValidation register
builder.Services.AddFluentValidationServices();

// MiddleWares
builder.Services.AddScoped<ExceptionMiddleware>();

// Add AutoMapper (.Domain Assembly)
builder.Services.AddAutoMapperServices();

// Chaching
builder.Services.AddCachingServices();

// Change in Production
// RateLimiter Services
builder.Services.AddRateLimiterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseCustomMiddlewares();

app.UseRateLimiter();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
