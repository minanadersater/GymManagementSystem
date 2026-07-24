using GymManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Context
{
    public class GymDbcontext:IdentityDbContext<ApplicationUser>
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
            base.OnModelCreating(modelBuilder);   

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Trainer>().OwnsOne(t => t.Address);

        }
        public DbSet<Plan> Plans { get; set; } 
        public DbSet<Member> Members { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Trainer> Trainers { get; set; }


       

    }
}
