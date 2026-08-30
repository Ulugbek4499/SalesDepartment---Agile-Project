using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Founders.Response
{
    public class FounderResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public ICollection<Contract> Contracts { get; set; }
    }
}
