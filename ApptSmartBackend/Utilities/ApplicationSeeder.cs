using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.AspNetCore.Identity;

namespace ApptSmartBackend.Utilities
{
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

        public static async Task SeedUsers(UserManager<AuthUser> userMangager, IRepositoryAsync<UserInfo> userInfoRepo)
        {
            
        }
    }
}
