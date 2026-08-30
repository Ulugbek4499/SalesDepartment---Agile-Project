using SalesDepartment.Domain.Commons;

namespace SalesDepartment.Domain.Entities
{
    public class Founder : BaseAuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
