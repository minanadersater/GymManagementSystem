using GymManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.DataSeeds
{
    public class IdentityDataSeeding
    {
        public static async Task SeedAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,   // ✅ اتغيرت من IdentityUser
            ILogger logger,
            CancellationToken ct = default)
        {
            try
            {
                bool HasUsers = userManager.Users.Any();
                bool HasRoles = roleManager.Roles.Any();
                if (HasUsers && HasRoles) return;

                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>
                    {
                        new IdentityRole() { Name = "SuperAdmin" },
                        new IdentityRole() { Name = "Admin" }
                    };

                    foreach (var roleName in Roles.Select(r => r.Name))
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                            if (!roleResult.Succeeded)   // ✅ اتصلحت
                            {
                                logger.LogError("Failed to create role {RoleName}", roleName);
                            }
                        }
                    }
                }

                if (!HasUsers)
                {
                    var MainUser = new ApplicationUser()
                    {
                        FirstName = "Mina",
                        LastName = "Nader",
                        UserName = "MianN",
                        Email = "Mina@gmail.com",
                        PhoneNumber = "01000000000",
                    };

                    var UserResult = await userManager.CreateAsync(MainUser, "Mina@123");
                    if (!UserResult.Succeeded)
                    {
                        logger.LogError("Failed to create user");
                        return;   // ✅ رجعنا فوراً لو فشل الإنشاء
                    }

                    await userManager.AddToRoleAsync(MainUser, "SuperAdmin");  // ✅ بقت بعد التأكد من النجاح
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding identity data.");
                throw;
            }
        }
    }
}