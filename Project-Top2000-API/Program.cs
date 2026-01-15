using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;
using TemplateJwtProject.Services;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Add services
// =======================

// Allow disabling database/identity during development
var useDatabase = builder.Configuration.GetValue<bool>("UseDatabase", false);
Console.WriteLine($"UseDatabase: {useDatabase}");

// -----------------------
// Database & Identity
// -----------------------
if (useDatabase)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
}

// -----------------------
// JWT Authentication
// -----------------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey is not configured");

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

// -----------------------
// CORS
// -----------------------
var corsSettings = builder.Configuration.GetSection("CorsSettings");
var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>()
    ?? new[]
    {
        "http://localhost:5173",
        "https://localhost:5173",
        "https://teamgeminiapi.runasp.net",
        "http://teamgeminiapi.runasp.net"
    };

builder.Logging.AddConsole();
builder.Services.AddSingleton(_ => allowedOrigins);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// -----------------------
// Application services
// -----------------------
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

builder.Services.AddControllers();

// =======================
// Swagger (MINIMAL)
// =======================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =======================
// Swagger middleware
// =======================
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

// =======================
// Role initialization
// =======================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RoleInitializer.InitializeAsync(services);
}

// =======================
// Middleware pipeline
// =======================
app.UseRouting();

app.UseCors("DefaultCorsPolicy");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
