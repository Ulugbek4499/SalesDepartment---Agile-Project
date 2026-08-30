using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Infrastructure.Persistence.Configurations
{
    public class HomeConfiguration : IEntityTypeConfiguration<Home>
    {
        public void Configure(EntityTypeBuilder<Home> builder)
        {
            builder.Property(t => t.ApartmentNumber)
              .HasMaxLength(200)
              .IsRequired();
        }
    }
}
