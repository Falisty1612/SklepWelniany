using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SklepWelniany.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }


        [Required]
        public int CartId { get; set; }


        [Required]
        public int ProductId { get; set; }


        public Cart Cart { get; set; }


        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
