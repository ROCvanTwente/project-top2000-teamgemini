using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;
using TemplateJwtProject.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURATIE & SERVICES
// ==========================================

// Database & Identity instellingen ophalen
var useDatabase = builder.Configuration.GetValue<bool>("UseDatabase", false);
Console.WriteLine($"UseDatabase: {useDatabase}");

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

// CORS configuratie
var corsSettings = builder.Configuration.GetSection("CorsSettings");
var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>()
                     ?? new[] { "http://localhost:5173", "https://localhost:5173" };

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
            .AllowCredentials();
    });
});

// Eigen Services registreren
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

// Controllers & Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // <--- Swagger toegevoegd

// Logging
builder.Logging.AddConsole();

var app = builder.Build();

// ==========================================
// 2. INITIALISATIE (Database & Seeding)
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var configuration = services.GetRequiredService<IConfiguration>();

    // Check of database aan staat
    var dbEnabled = configuration.GetValue<bool>("UseDatabase", false);

    if (dbEnabled)
    {
        try
        {
            var dbContext = services.GetRequiredService<AppDbContext>();
            // Automatisch database en tabellen aanmaken indien nodig
            await dbContext.Database.MigrateAsync();

            // Rollen en admin users aanmaken
            await RoleInitializer.InitializeAsync(services);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij initialiseren database: {ex.Message}");
        }
    }
}

// ==========================================
// 3. MIDDLEWARE PIPELINE
// ==========================================

// Swagger UI activeren (zodat je endpoints kunt zien op /swagger)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// CORS: moet tussen Routing en Auth staan
app.UseCors("DefaultCorsPolicy");

// HTTPS Redirection (alleen in productie om CORS-issues in development te voorkomen)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();