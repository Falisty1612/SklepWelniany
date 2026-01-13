using Microsoft.AspNetCore.Identity;

namespace SklepWelniany.Repositories
{
    public class CartRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AddItem(int productId, int qty)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                string userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new Cart
                    {
                        UserId = userId
                    };
                    _db.Carts.Add(cart);
                }
                _db.SaveChanges();
                //cart details:
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.CartId == cart.Id && a.ProductId == productId);
                if(cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    cartItem = new CartDetail
                    {
                        ProductId = productId,
                        CartId = cart.Id,
                        Quantity = qty
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> RemoveItem(int productId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            try
            {
                string userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    return false;
                }
                //cart details:
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.CartId == cart.Id && a.ProductId == productId);
                if(cartItem is null)
                {
                    return false;
                }
                else if (cartItem.Quantity == 1)
                {
                    _db.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity -= 1;
                }
                _db.SaveChanges();
                //transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<IEnumerable<Cart>> GetUserCart()
        {
            var userId = GetUserId();
            if(userId == null)
            {
                throw new Exception("Invalid user");
            }
            var cart = await _db.Carts.Include(a => a.CartDetails).ThenInclude(a => a.Product).ThenInclude(a => a.Type).Where(a => a.UserId == userId).ToListAsync;
            return Cart;

        }

        private async Task<Cart> GetCart(string userId)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

    }
}
