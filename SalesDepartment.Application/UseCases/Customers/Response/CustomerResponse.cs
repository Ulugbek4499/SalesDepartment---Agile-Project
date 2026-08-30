using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Customers.Response
{
    public class CustomerResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Passport { get; set; }
        public string PassportIssuedBy { get; set; }
        public string Address { get; set; }
        public string PhoneNumberOne { get; set; }
        public string PhoneNumberTwo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public ICollection<Contract> Contracts { get; set; }
    }
}
