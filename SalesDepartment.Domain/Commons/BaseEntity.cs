using System.ComponentModel.DataAnnotations.Schema;

namespace SalesDepartment.Domain.Commons
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
