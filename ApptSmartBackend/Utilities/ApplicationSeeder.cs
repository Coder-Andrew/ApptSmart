using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ApptSmartBackend.Utilities
{
    // Should probably move this class into the console app for seeding
    public static class ApplicationSeeder
    {
        public static async Task SeedIdentityRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User", "Manager" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task<List<string>> SeedUsers(UserManager<AuthUser> userManager, AppDbContext appContext, SeedUserInfo[] seedData, string seedUserPw)
        {
            try
            {
                List<string> aspNetUserIds = new();
                foreach (var user in seedData)
                {
                    string userId = await SaveAuthUser(userManager, user, seedUserPw);
                    UserInfo userInfo = CreateUserInfo(userId, user);
                    await SaveAppUser(appContext, userInfo);
                    aspNetUserIds.Add(userId);
                }
                return aspNetUserIds;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to seed users: {ex.Message}");
            }
        }

        public static async Task RemoveSeededUsers(UserManager<AuthUser> userManager, AppDbContext appContext, List<string> aspNetUserIds)
        {
            foreach (var id in aspNetUserIds)
            {
                AuthUser? authUser = await userManager.FindByIdAsync(id);
                if (authUser != null) await userManager.DeleteAsync(authUser);
                UserInfo userInfo = await appContext.UserInfos.FirstAsync(ui => ui.AspNetIdentityId == id);
                var appts = userInfo.UserAppointments;
                if (userInfo != null)
                {
                    RemoveAppUserNavProps(appContext, userInfo);
                    appContext.UserInfos.Remove(userInfo);
                    await appContext.SaveChangesAsync();
                }
            }
        }

        public static async Task<List<string>> GetSeededUserIds(UserManager<AuthUser> userManager, AppDbContext appContext, SeedUserInfo[] seedData)
        {
            List<string> userIds = new();
            foreach (SeedUserInfo seededUser in seedData)
            {
                string email = $"{seededUser.FirstName}{seededUser.LastName}@example.com";
                AuthUser? user = await userManager.FindByEmailAsync(email);
                if (user == null) continue;
                userIds.Add(user.Id);
            }
            return userIds;
        }

        private static AuthUser CreateAuthUser(SeedUserInfo userInfo)
        {
            string email = $"{userInfo.FirstName}{userInfo.LastName}@example.com";
            return new AuthUser
            {
                Email = email,
                UserName = email,
            };
        }

        private static UserInfo CreateUserInfo(string aspNetId, SeedUserInfo userInfo)
        {
            return new UserInfo
            {
                AspNetIdentityId = aspNetId,
                UserAppointments = userInfo.Appointments,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName
            };
        }

        private static async Task<string> SaveAuthUser(UserManager<AuthUser> userManager, SeedUserInfo userInfo, string seedUserPw)
        {
            var authUser = CreateAuthUser(userInfo);
            var user = await userManager.FindByEmailAsync(authUser.Email!);
            if (user == null)
            {
                await userManager.CreateAsync(authUser, seedUserPw);
                await userManager.AddToRoleAsync(authUser, userInfo.Role);
            }
            if (authUser == null)
            {
                throw new InvalidOperationException($"The user was not seeded properly, password may not be strong enough or user may have an invalid email address");
            }

            return authUser.Id;
        }

        private static async Task SaveAppUser(AppDbContext appContext, UserInfo userInfo)
        {
            if (appContext.UserInfos.Any(ui => ui.AspNetIdentityId == userInfo.AspNetIdentityId)) return;

            await appContext.UserInfos.AddAsync(userInfo);
            await appContext.SaveChangesAsync();
        }

        private static void RemoveAppUserNavProps(AppDbContext appContext, UserInfo userInfo)
        {
            var entry = appContext.Entry(userInfo);

            foreach (var navigation in entry.Navigations)
            {
                if (navigation.Metadata.IsCollection)
                {
                    var navCurrentValue = navigation.CurrentValue;
                    if (navCurrentValue != null)
                    {
                        var relatedEntities = (IEnumerable<object>)navCurrentValue;
                        if (relatedEntities != null)
                        {
                            appContext.RemoveRange(relatedEntities);
                        }
                    }
                }
            }
        }
    }
}
