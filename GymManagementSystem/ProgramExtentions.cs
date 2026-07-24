using GymManagementSystem.DAL.Context;
using GymManagementSystem.DAL.DataSeeds;
using GymManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem
{
    public static class ProgramExtentions
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDbcontext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var pending = await dbcontext.Database.GetPendingMigrationsAsync();

            if (pending.Any())
            {
                logger.LogInformation("Applying pending migrations...");
                await dbcontext.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
            }

            var seedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbcontext, seedPath, logger);   // ✅ GymDataSeeding مش GymDataSeed
            await IdentityDataSeeding.SeedAsync(RoleManager, UserManager, logger);
        }
    }
}