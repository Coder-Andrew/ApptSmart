using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AptSmartBackend.Models;
using AptSmartBackend.Services.Abstract;
using AptSmartBackend.Services.Concrete;
using AptSmartBackend.Helpers;
using AptSmartBackend.SettingsObjects;
using AptSmartBackend.Utilities;
using AptSmartBackend.DAL.Abstract;
using AptSmartBackend.DAL.Concrete;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();


// Get connection strings/secrets
string authConnectionString = builder.Configuration.GetConnectionString("AuthConnection") ?? throw new KeyNotFoundException("Cannot find Auth Connection string");
string appConnectionString = builder.Configuration.GetConnectionString("AppConnection") ?? throw new KeyNotFoundException("Cannot find App Connection string");

IConfiguration jwtConfig = builder.Configuration.GetSection("Jwt");
builder.Services
    .AddOptions<JwtSettings>()
    .Bind(jwtConfig)
    .ValidateDataAnnotations()
    .ValidateOnStart();

string jwtSecret = jwtConfig["Secret"] ?? throw new KeyNotFoundException("Cannot find JWT Secret");

// CorsSettings corsSettings = builder.Configuration.GetSection("Cors").Get<CorsSettings>() ?? throw new KeyNotFoundException("Cannot find CORS settings");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure CORS // CHANGE IN PRODUCTIONS
//string corsPolicyName = "frontendCorsPolicy";

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(corsPolicyName, policy =>
//    {
//        foreach (string site in corsSettings.AllowedSites)
//        {
//            policy.WithOrigins(site)
//                .AllowAnyHeader()
//                .AllowAnyMethod()
//                .AllowCredentials();
//        }
//    });
//});

// Add Identity with sql server
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(authConnectionString));

// Setup appdbcontext/app connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(appConnectionString));

// Setup Identity
builder.Services.AddIdentity<AuthUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// Setup JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; // CHANGE IN PRODUCTIONS
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = false,     // CHANGE IN PRODUCTIONS
        ValidateAudience = false,   // CHANGE IN PRODUCTIONS
        ClockSkew = TimeSpan.Zero 
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("AuthToken", out string? authToken))
            {
                context.Token = authToken;
            }

            return Task.CompletedTask;
        }
    };
});

// Dependency Injection
builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<JwtHelper>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentitySeeder.SeedRolesAsync(userManager);
}

//app.UseCors(corsPolicyName); // CHANGE IN PRODUCTIONS

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();