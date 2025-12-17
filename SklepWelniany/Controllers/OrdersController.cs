using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SklepWelniany.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SklepWelniany.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly SklepWelnianyDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(SklepWelnianyDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Orders/Create
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _db.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            // create order from cart
            var order = new Order
            {
                UserId = userId,
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (var item in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                _db.OrderItems.Add(orderItem);
            }

            // clear cart
            _db.CartItems.RemoveRange(cart.Items);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", new { id = order.Id });
        }

        // GET: /Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _db.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();

            return View(order);
        }
    }
}
