using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Context
{
    public class GymDbcontext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           //optionsBuilder.UseSqlServer(@"Server=.;Database=GymManagementSystem;Trusted_Connection=True;trustservercertificate=true;");

            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Gym_ManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new Configurations.PlanConfiguration());
        }
        public DbSet<Plan> Plans { get; set; }
    }
}
