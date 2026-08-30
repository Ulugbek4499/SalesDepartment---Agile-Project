using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Infrastructure.Persistence.Configurations
{
    public class PaymentTypeConfiguration : IEntityTypeConfiguration<PaymentType>
    {
        public void Configure(EntityTypeBuilder<PaymentType> builder)
        {
            builder.Property(t => t.Name)
              .HasMaxLength(200)
              .IsRequired();
        }
    }
}
