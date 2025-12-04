using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;
using TemplateJwtProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database configuratie
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity configuratie
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication configuratie
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };
});

// CORS configuratie - read origins from configuration
var corsSettings = builder.Configuration.GetSection("CorsSettings");
var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>() 
                     ?? new[] { "http://localhost:3000", "http://localhost:5173", "http://localhost:5237", "https://teamgeminiapi.runasp.net" };

builder.Logging.AddConsole();
builder.Services.AddSingleton(_ => allowedOrigins);
Console.WriteLine("CORS allowed origins: " + string.Join(", ", allowedOrigins));

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policyBuilder =>
    {
        policyBuilder
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            // If your frontend uses cookies or fetch(..., { credentials: 'include' })
            // enable credentials (and ensure origins are explicit, not '*')
            .AllowCredentials();
    });
});

// Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

builder.Services.AddControllers();
// Use Swagger/OpenAPI compatible with .NET 8
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialiseer rollen - tolerant when DB is not available (for local testing without DB)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await RoleInitializer.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetService<ILogger<Program>>();
        logger?.LogWarning(ex, "Role initialization skipped because the database is not available. Starting without Identity DB for local testing.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Only redirect HTTP->HTTPS in non-development environments to avoid breaking CORS preflight locally
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// IMPORTANT: call __UseCors__ before authentication/authorization
app.UseCors("DefaultCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
