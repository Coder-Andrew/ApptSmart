using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ApptSmartBackend.Utilities
{
    // TODO: Need to adjust nav property remover method to correctly remove associated appointment
    public static class ApplicationSeeder
    {
        public static async Task SeedIdentityRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User", "CompanyOwner" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task<Company> CreateSeedCompany(AppDbContext appContext, UserInfo owner, string companyName, string companySlug, string description)
        {
            var existing = await appContext.Companies.FirstOrDefaultAsync(c => c.CompanySlug == companySlug);
            if (existing != null) return existing;

            var company = new Company
            {
                CompanyName = companyName,
                CompanySlug = companySlug,
                CompanyDescription = description,
                OwnerId = owner.Id
            };

            appContext.Companies.Add(company);
            await appContext.SaveChangesAsync();
            return company;
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

                var userInfo = await appContext.UserInfos
                    .Include(u => u.UserAppointments)
                        .ThenInclude(ua => ua.Appointment) // Eager load the nested appointment
                    .FirstOrDefaultAsync(ui => ui.AspNetIdentityId == id);

                if (userInfo != null)
                {
                    await RemoveAppUserNavProps(appContext, userInfo);
                    appContext.UserInfos.Remove(userInfo);
                    await appContext.SaveChangesAsync();
                }
            }
        }

        private static async Task RemoveAppUserNavProps(AppDbContext appContext, UserInfo userInfo)
        {
            await RemoveUsersAppts(appContext, userInfo);
        }

        private static async Task RemoveUsersAppts(AppDbContext appContext, UserInfo userInfo)
        {
            foreach (var userAppt in userInfo.UserAppointments.ToList()) // ToList() avoids modifying the collection during iteration
            {
                var appointmentId = userAppt.AppointmentId;

                // Remove the UserAppointment link
                appContext.UserAppointments.Remove(userAppt);

                // Check if any other users are still linked to this appointment
                bool isOrphaned = !await appContext.UserAppointments
                    .AnyAsync(ua => ua.AppointmentId == appointmentId && ua.UserInfoId != userInfo.Id);

                if (isOrphaned)
                {
                    var appointment = await appContext.Appointments.FindAsync(appointmentId);
                    if (appointment != null)
                    {
                        appContext.Appointments.Remove(appointment);
                    }
                }
            }
            await appContext.SaveChangesAsync();
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
                foreach (var role in userInfo.Roles)
                {
                    await userManager.AddToRoleAsync(authUser, role);
                }
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
    }
}
