using Microsoft.EntityFrameworkCore;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Founder> Founders { get; set; }
        public DbSet<Home> Homes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
