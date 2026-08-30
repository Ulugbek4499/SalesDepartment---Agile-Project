using SalesDepartment.Domain.Commons;

namespace SalesDepartment.Domain.Entities
{
    public class Home : BaseAuditableEntity
    {
        public string Block { get; set; }
        public int Entrance { get; set; }
        public int Floor { get; set; }
        public int ApartmentNumber { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Area { get; set; }

        public virtual Contract? Contract { get; set; }
    }
}