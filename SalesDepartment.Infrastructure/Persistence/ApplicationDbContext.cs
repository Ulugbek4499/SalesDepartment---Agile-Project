using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;
using SalesDepartment.Infrastructure.Persistence.Interceptors;

namespace SalesDepartment.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

        public ApplicationDbContext(
          DbContextOptions<ApplicationDbContext> options,
          AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
          : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Founder> Founders { get; set; }
        public DbSet<Home> Homes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }
    }
}
