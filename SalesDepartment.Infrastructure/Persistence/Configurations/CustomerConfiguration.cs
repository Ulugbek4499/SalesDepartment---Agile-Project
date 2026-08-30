using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(t => t.FirstName)
              .HasMaxLength(200)
              .IsRequired();
        }
    }
}
