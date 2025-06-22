using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
//using ApptSmartBackend;
using ApptSmartBackend.Models;
using ApptSmartBackend.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Proxies;


class Program
{
    static async Task Main(string[] args)
    {
        string[] commands = { "RemoveSeedData", "SeedUserData" };
        if (args.Length <= 0)
        {
            Console.WriteLine($"Please provide a command: {string.Join(", ", commands)}");
            return;
        }

        var command = args[0].ToLower();

        if (!commands.Any(c => string.Equals(c.ToLower(), command)))
        {
            Console.WriteLine($"{command} is not a valid command");
            return;
        }

        // Setup and load user-secrets
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

        string authConnectionString = configuration.GetConnectionString("AuthConnection") ?? throw new KeyNotFoundException("Cannot find Auth Connection string");
        string appConnectionString = configuration.GetConnectionString("AppConnection") ?? throw new KeyNotFoundException("Cannot find App Connection string");
        string seedUserPw = configuration["SeedUserPassword"] ?? throw new KeyNotFoundException("Cannot find Seed User Password");

        // Setup services
        var services = new ServiceCollection();

        // Inject config so contexts can access them
        services.AddSingleton<IConfiguration>(configuration);

        // Add logging for Identity
        services.AddLogging(options => options.AddConsole());

        // Setup auth db
        services.AddDbContext<AuthDbContext>(options => 
        {
            options.UseSqlServer(authConnectionString);
        });

        // Setup Identity
        services.AddIdentity<AuthUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        // Setup app db and use lazy loading to populate nav properties
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(appConnectionString);
            options.UseLazyLoadingProxies();
        });

        using var serviceProvider = services.BuildServiceProvider(validateScopes: true);

        using (var scope = serviceProvider.CreateScope())
        {
            var scopedProvider = scope.ServiceProvider;

            var appDbContext = scopedProvider.GetRequiredService<AppDbContext>();
            var userManager = scopedProvider.GetRequiredService<UserManager<AuthUser>>();

            // Get database objects
            var userSeedData = SeedData.UserSeedData;


            switch (command)
            {
                case "removeseeddata":
                    var seededUserIds = await ApplicationSeeder.GetSeededUserIds(userManager, appDbContext, userSeedData);
                    await ApplicationSeeder.RemoveSeededUsers(userManager, appDbContext, seededUserIds);
                    return;

                case "seeduserdata":
                    await ApplicationSeeder.SeedUsers(userManager, appDbContext, userSeedData, seedUserPw);
                    return;
                
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    return;
            }
        }       
    }
}
