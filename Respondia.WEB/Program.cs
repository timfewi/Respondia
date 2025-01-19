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


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Automatic DI register
builder.Services.AddInjectables(
    Assembly.GetExecutingAssembly(),
    typeof(EntryPointCore).Assembly,
    typeof(EntryPointBusinessLogic).Assembly
);

// Appsettings.json config
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

// Serilog config
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Identity Components
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(24);
});


//builder.Services.AddTransient<IEmailSender<AppUser>, EmailSenderService>();
builder.Services.AddTransient<EmailSenderService>();
builder.Services.AddSingleton<EmailQueueService>();
builder.Services.AddHostedService<EmailProcessorService>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddSingleton<SmtpSettings>(sp => sp.GetRequiredService<IOptions<SmtpSettings>>().Value);

// HttpContextAccesor
builder.Services.AddHttpContextAccessor();

// FluentValidation register
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// register Validators
builder.Services.AddValidatorsFromAssemblyContaining<EntryPointCore>();

// MiddleWares
builder.Services.AddScoped<ExceptionMiddleware>();



// Add AutoMapper (.Domain Assembly)
builder.Services.AddAutoMapper(
    typeof(EntryPointBusinessLogic).Assembly,
    typeof(EntryPointCore).Assembly
    );

// Suppress the diagnostic warning for AddHybridCache
#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache();

builder.Services.AddHybridCache(options =>
{
    options.MaximumPayloadBytes = 1024 * 1024; // max-size of caching entry (1 MB)
    options.MaximumKeyLength = 1024; // max key length
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(5), // expiration time of the cache entry
        LocalCacheExpiration = TimeSpan.FromMinutes(5) // expiration time of the local cache entry
    };
});

// Change in Production
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("Default", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 50
            }));
});

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
