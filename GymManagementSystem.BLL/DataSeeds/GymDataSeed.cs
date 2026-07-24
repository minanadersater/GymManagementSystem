using GymManagementSystem.DAL.Context;
using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.DataSeeds
{
    public static class GymDataSeed
    {
        public static async Task SeedAsync(GymDbcontext dbcontext,String seedFilesPath,ILogger logger,CancellationToken ct)
        {
            try
            {
                if (!await dbcontext.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>("Plans.json", seedFilesPath);
                    if (plans.Count > 0)
                    {
                        dbcontext.Plans.AddRange(plans);
                        logger.LogInformation($"Seeding {plans.Count} Plans data...");
                    }
                }
                if (dbcontext.ChangeTracker.HasChanges())
                {
                    await dbcontext.SaveChangesAsync(ct);
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }

        }
        private static List<T> LoadDataFromJsonFile<T>(string fileName,string FolderPath)
        {
          var filePath = Path.Combine(FolderPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{fileName}' was not found in the folder '{FolderPath}'.");
            }
            var Data = File.ReadAllText(filePath);
            var Options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();
        }
    }
}
