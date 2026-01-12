using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SklepWelniany.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SklepWelniany.Controllers
{
    [Authorize] // Tylko zalogowani mogą tu wejść
    public class AccountController : Controller
    {
        private readonly SklepWelnianyDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SklepWelnianyDbContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            // Pobierz zamówienia użytkownika
            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product) // Załaduj szczegóły produktów
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate) // Najnowsze na górze
                .ToListAsync();

            var model = new AccountViewModel
            {
                Orders = orders,
                ChangePassword = new ChangePasswordModel()
            };

            return View(model);
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(AccountViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            // Jeśli walidacja hasła przeszła
            if (ModelState.IsValid)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.ChangePassword.OldPassword, model.ChangePassword.NewPassword);

                if (changePasswordResult.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user); // Odśwież sesję po zmianie hasła
                    TempData["SuccessMessage"] = "Hasło zostało zmienione pomyślnie.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Jeśli coś poszło nie tak, musimy ponownie załadować listę zamówień, żeby wyświetlić widok
            model.Orders = await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View("Index", model);
        }
    }
}