using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Payments.Response
{
    public class PaymentResponse
    {
        public int Id { get; set; }
        public string PaymentNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; }
        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
