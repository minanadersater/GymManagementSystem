using GymManagementSystem.DAL.Context;
using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.DataSeeds
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(
            GymDbcontext dbContext,
            string seedFilesPath,
            ILogger logger,
            CancellationToken ct = default)
        {
            try
            {
                if (!await dbContext.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json", seedFilesPath);
                    if (plans.Count > 0)
                    {
                        dbContext.Plans.AddRange(plans);
                        logger.LogInformation("Seeded {Count} plans.", plans.Count);
                    }
                }

                if (dbContext.ChangeTracker.HasChanges())
                    await dbContext.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gym data seeding failed.");
                throw;
            }
        }

        private static List<T> LoadDataFromJsonFile<T>(string fileName, string folderPath)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Seed data file not found: {filePath}");

            var data = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
        }
    }
}