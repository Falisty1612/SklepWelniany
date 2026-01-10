using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SklepWelniany.Models
{
    public class AccountViewModel
    {
        // Sekcja: Historia zamówień
        public List<Order> Orders { get; set; }

        // Sekcja: Zmiana hasła
        public ChangePasswordModel ChangePassword { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Aktualne hasło jest wymagane")]
        [DataType(DataType.Password)]
        [Display(Name = "Aktualne hasło")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nowe hasło jest wymagane")]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Nowe hasło i potwierdzenie nie są zgodne.")]
        public string ConfirmPassword { get; set; }
    }
}