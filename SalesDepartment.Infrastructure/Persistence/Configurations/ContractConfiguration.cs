using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Infrastructure.Persistence.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.Property(t => t.ContractNumber)
              .HasMaxLength(200)
              .IsRequired();
        }
    }

}
