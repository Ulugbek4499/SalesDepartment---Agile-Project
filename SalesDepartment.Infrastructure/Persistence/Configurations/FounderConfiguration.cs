using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Infrastructure.Persistence.Configurations
{
    public class FounderConfiguration : IEntityTypeConfiguration<Founder>
    {
        public void Configure(EntityTypeBuilder<Founder> builder)
        {
            builder.Property(t => t.FirstName)
              .HasMaxLength(200)
              .IsRequired();
        }
    }
}
