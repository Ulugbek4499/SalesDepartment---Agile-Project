using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Contracts.Response
{
    public class ContractResponse
    {
        public int Id { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime PaymentStartDate { get; set; }

        public decimal TotalAmountOfContract { get; set; }
        public decimal InAdvancePaymentOfContract { get; set; }
        public int NumberOfMonths { get; set; }
        public decimal AmountOfMonthlyPayment { get; set; }

        public int HomeId { get; set; }
        public Home Home { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int FounderId { get; set; }
        public Founder Founder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public ICollection<Payment> Payments { get; set; }

        public decimal? DeptAmout { get; set; }
        public Dictionary<DateTime, decimal>? ScheduledInfo { get; set; }
        public Dictionary<DateTime, decimal>? ActualInfo { get; set; }
      /*  public Dictionary<string, decimal>? ScheduledInfo2 { get; set; }*/
    }
}
