using SalesDepartment.Domain.Commons;

namespace SalesDepartment.Domain.Entities
{
    public class Contract : BaseAuditableEntity
    {
        public string ContractNumber { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime PaymentStartDate { get; set; }
        
        public decimal TotalAmountOfContract { get; set; }
        public decimal InAdvancePaymentOfContract { get; set; } = 0;
        public int NumberOfMonths { get; set; } = 0;
        public decimal AmountOfMonthlyPayment => CalculateMonthlyPayment();
      
        public int HomeId { get; set; }
        public virtual Home Home { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int FounderId { get; set; }
        public virtual Founder Founder { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        private decimal CalculateMonthlyPayment()
        {
            return (TotalAmountOfContract - InAdvancePaymentOfContract) / NumberOfMonths;
        }
    }
}
