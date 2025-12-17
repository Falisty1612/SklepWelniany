using System.ComponentModel.DataAnnotations;

namespace SklepWelniany.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa produktu jest wymagana")]
        [StringLength(100, ErrorMessage = "Nazwa nie może mieć więcej niż 100 znaków")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Opis nie może mieć więcej niż 500 znaków")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana")]
        [Range(0, 10000, ErrorMessage = "Cena musi być między 0 a 10 000")]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
