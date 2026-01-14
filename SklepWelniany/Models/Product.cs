using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SklepWelniany.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(80)]
        public string ProductName { get; set; }


        [Required]
        public double Price { get; set; }


        public string? Image { get; set; }


        [Required]
        public int TypeId { get; set; }


        public Type Type { get; set; }


        public List<OrderDetail> OrderDetails { get; set; }


        public List<CartDetail> CartDetails { get; set; }

        public Stock Stock { get; set; }

        [NotMapped]
        public string TypeName { get; set; }
    }
}
