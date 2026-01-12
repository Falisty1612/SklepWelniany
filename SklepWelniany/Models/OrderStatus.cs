using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SklepWelniany.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int Id { get; set; }


        [Required]
        public int StatusId { get; set; }


        [Required, MaxLength(30)]
        public string ?StatusName { get; set; }

    }
}
