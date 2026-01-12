using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging; // <--- 1. WA¯NE: Dodano ten using

namespace SklepWelniany.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginModel> _logger; // <--- 2. Dodano pole loggera

        // 3. Zaktualizowany konstruktor
        public LoginModel(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger; // <--- Przypisanie loggera
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username or email")]
            public string Login { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // 1. Szukamy u¿ytkownika po Loginie LUB Emailu
                var user = await _userManager.FindByNameAsync(Input.Login);
                if (user == null && Input.Login.Contains("@"))
                {
                    user = await _userManager.FindByEmailAsync(Input.Login);
                }

                // 2. ZABEZPIECZENIE: Jeœli u¿ytkownik nie istnieje
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Nieprawid³owy login lub has³o.");
                    return Page();
                }

                // 3. Próba logowania
                // Wa¿ne: PasswordSignInAsync wymaga UserName, dlatego pobieramy go ze znalezionego usera
                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("U¿ytkownik zalogowany."); // Teraz to zadzia³a!
                    return LocalRedirect("~/");
                }

                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nieprawid³owy login lub has³o.");
                    return Page();
                }
            }

            // Jeœli dotarliœmy tutaj, to walidacja formularza nie przesz³a
            return Page();
        }
    }
}