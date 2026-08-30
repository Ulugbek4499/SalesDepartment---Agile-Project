using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Homes.Response
{
    public class HomeResponse
    {
        public int Id { get; set; }
        public string Block { get; set; }
        public int Entrance { get; set; }
        public int Floor { get; set; }
        public int ApartmentNumber { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Area { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public int? ContractId { get; set; }
        public Contract? Contract { get; set; }
    }
}
