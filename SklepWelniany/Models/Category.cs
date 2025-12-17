using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SklepWelniany.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Relacja 1 do wielu: jedna kategoria może mieć wiele produktów
        public ICollection<Product> Products { get; set; }
    }
}
