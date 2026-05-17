using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Description).HasColumnType("varchar(50)");
            builder.Property(p => p.Name).HasColumnType("varchar(50)").IsRequired();
            builder.Property(p => p.Price).HasPrecision(18, 2);
            builder.Property(p => p.CreateAt).HasDefaultValueSql("GETDATE()");
            builder.ToTable(tb => 
            {
                tb.HasCheckConstraint("DurationCheckValue", "DurationDays between 0 and 365");  
            });
        }
    }


}
