using System.ComponentModel.DataAnnotations;

namespace SklepWelniany.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        [StringLength(200)]
        public string Url { get; set; }

        public bool IsPrimary { get; set; }
    }
}
