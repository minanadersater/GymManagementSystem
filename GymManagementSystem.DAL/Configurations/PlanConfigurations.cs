using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Configurations
{
    public class PlanConfigurations:IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name)
                   .HasColumnType("varchar(50)");

            builder.Property(p => p.Description)
                   .HasMaxLength(200);

            builder.Property(p => p.Price)
                   .HasPrecision(10, 2);

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    "DurationCheckValue",
                    "[DurationDays] BETWEEN 1 AND 365"
                );
            });
        }
    }
}
