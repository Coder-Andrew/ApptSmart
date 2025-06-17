using ApptSmartBackend.Helpers;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApptSmartBackend.Utilities
{
    public static class SimpleDevSeeder
    {
        public static async Task SeedApp(
            string seedPassword,
            AppDbContext appContext,
            UserManager<AuthUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            // TODO: Expand on later
            if (await userManager.Users.AnyAsync()) return;

            string[] roles = { "Admin", "User", "CompanyOwner" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var admin = new AuthUser
            {
                Email = "admin@test.com",
                UserName = "admin@test.com",
                EmailConfirmed = true
            };

            // If admin exists, skip seeding
            if (await userManager.FindByEmailAsync(admin.Email) != null) return;

            await userManager.CreateAsync(admin, seedPassword);
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "CompanyOwner" });

            var adminUserInfo = new UserInfo
            {
                AspNetIdentityId = admin.Id,
                FirstName = "admin",
                LastName = "admin",
            };

            await appContext.UserInfos.AddAsync(adminUserInfo);
            await appContext.SaveChangesAsync();

            string adminCompanyName = "ApptSmart";

            var adminCompany = new Company
            {
                CompanyName = adminCompanyName,
                CompanyDescription = "The default appointment management solution for ApptSmart! Book an appointment with our representatives below!",
                CompanySlug = SlugHelper.Slugify(adminCompanyName),
                Owner = adminUserInfo
            };

            await appContext.Companies.AddAsync(adminCompany);
            await appContext.SaveChangesAsync();

            List<Appointment> adminAppointments = new List<Appointment>
            {
                new Appointment
                {
                    Company = adminCompany,
                    StartTime = DateTime.Now.AddDays(5),
                    EndTime = DateTime.Now.AddDays(5).AddMinutes(30)
                },
                new Appointment
                {
                    Company = adminCompany,
                    StartTime = DateTime.Now.AddDays(5).AddHours(1),
                    EndTime = DateTime.Now.AddDays(5).AddHours(1).AddMinutes(30)
                },
                new Appointment
                {
                    Company = adminCompany,
                    StartTime = DateTime.Now.AddDays(6),
                    EndTime = DateTime.Now.AddDays(6).AddMinutes(75)
                },
                new Appointment
                {
                    Company = adminCompany,
                    StartTime = DateTime.Now.AddDays(7),
                    EndTime = DateTime.Now.AddDays(7).AddHours(1)
                }
            };

            await appContext.Appointments.AddRangeAsync(adminAppointments);
            await appContext.SaveChangesAsync();

            var johnDoe = new AuthUser
            {
                Email = "jd@example.com",
                UserName = "jd@example.com",
            };

            await userManager.CreateAsync(johnDoe, seedPassword);
            await userManager.AddToRoleAsync(johnDoe, "user");

            var johnDoeUserInfo = new UserInfo
            {
                AspNetIdentityId = johnDoe.Id,
                FirstName = "John",
                LastName = "Doe"
            };

            await appContext.UserInfos.AddAsync(johnDoeUserInfo);
            await appContext.SaveChangesAsync();
        }
    }
}