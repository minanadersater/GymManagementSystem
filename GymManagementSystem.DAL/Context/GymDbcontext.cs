using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Context
{
    public class GymDbcontext:DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Gym_ManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;");

        //}

        public GymDbcontext(DbContextOptions<GymDbcontext> options) : base(options)
        {
             
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Plan> Plans { get; set; } 
        public DbSet<Member> Members { get; set; }
    }
}
