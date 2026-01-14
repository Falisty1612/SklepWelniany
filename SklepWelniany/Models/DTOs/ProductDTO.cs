using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SklepWelniany.Models.DTOs
{
    public class ProductDTO
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

        public IFormFile? ImageFile { get; set; }
        
        public IEnumerable<SelectListItem>? TypeList { get; set; }
    }
}
