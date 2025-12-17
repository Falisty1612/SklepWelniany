using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SklepWelniany.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  // powiązanie z użytkownikiem

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Lista produktów w zamówieniu
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
