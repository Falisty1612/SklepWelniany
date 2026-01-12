using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SklepWelniany.Models
{
    [Table("Type")]
    public class Type
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(40)]
        public string ProductType { get; set; }
        public List<Product> Products { get; set; }
    }
}