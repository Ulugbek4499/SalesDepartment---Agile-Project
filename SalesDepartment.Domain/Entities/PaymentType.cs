using SalesDepartment.Domain.Commons;

namespace SalesDepartment.Domain.Entities
{
    public class PaymentType : BaseAuditableEntity
    {
        public string Name { get; set; }
    }
}
