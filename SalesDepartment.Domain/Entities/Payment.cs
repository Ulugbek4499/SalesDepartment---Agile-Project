using SalesDepartment.Domain.Commons;

namespace SalesDepartment.Domain.Entities
{
    public class Payment : BaseAuditableEntity
    {
        public string PaymentNumber { get; set; }
        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public int PaymentTypeId { get; set; }
        public virtual PaymentType PaymentType { get; set; }
    }
}
