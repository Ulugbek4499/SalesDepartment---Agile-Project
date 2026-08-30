using SalesDepartment.Domain.Commons;

namespace SalesDepartment.Domain.Entities
{
    public class Customer : BaseAuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Passport { get; set; }
        public string PassportIssuedBy { get; set; }
        public string Address { get; set; }
        public string PhoneNumberOne { get; set; }
        public string PhoneNumberTwo { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
