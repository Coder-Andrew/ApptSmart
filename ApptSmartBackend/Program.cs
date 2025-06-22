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
using ApptSmartBackend.Models;
using ApptSmartBackend.Services.Abstract;
using ApptSmartBackend.Services.Concrete;
using ApptSmartBackend.Helpers;
using ApptSmartBackend.SettingsObjects;
using ApptSmartBackend.Utilities;
using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.DAL.Concrete;
using Microsoft.AspNetCore.Authentication;
using ApptSmartBackend.Extensions;


var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

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

CorsSettings corsSettings = builder.Configuration.GetSection("Cors").Get<CorsSettings>() ?? throw new KeyNotFoundException("Cannot find CORS settings");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();


// Configure CORS // CHANGE IN PRODUCTION
string corsPolicyName = "TrustedFrontendClients";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy.WithOrigins(corsSettings.AllowedSites.ToArray())
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Add Identity with sql server
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(authConnectionString));

var env = builder.Environment.EnvironmentName;

// Setup appdbcontext/app connection
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(appConnectionString);
});

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
//builder.Services.AddScoped<DbContext, TestDbContext>();
//builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
builder.Services.AddScoped<IUserAppointmentRepository, UserAppointmentRepository>();
builder.Services.AddScoped<IUserInfoRepositoryAsync, UserInfoRepositoryAsync>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IUserHelperService, UserHelperService>();
builder.Services.AddScoped<IUserInfoRepositoryAsync, UserInfoRepositoryAsync>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICompanyOwnerService, CompanyOwnerService>();
builder.Services.AddScoped<IUserAppointmentService, UserAppointmentService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddSingleton<JwtHelper>();

var app = builder.Build();

// Seed basic Identity roles
using (var scope = app.Services.CreateScope())
{
    // TODO: Move off into its own class/method
    string seedPw = builder.Configuration["SeedUserPassword"] ?? throw new KeyNotFoundException("Cannot find seed password");
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<AuthUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var appConext = services.GetRequiredService<AppDbContext>();
    
    await SimpleDevSeeder.SeedApp(seedPw, appConext, userManager, roleManager);
    //await ApplicationSeeder.SeedIdentityRolesAsync(userManager); // TODO: UPDATE LATER, MOVING TO SIMPLER SEEDING LOGIC FOR DEV

}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Company validation middleware to ensure company slug exists across multiple controllers that will use a company slug
app.UseCompanyValidation();

app.MapControllers();


app.Run();
