using Microsoft.AspNetCore.Identity;

namespace SklepWelniany.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderRepository(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IEnumerable<Order>> UserOrders()
        {
            
            var userId = GetUserId();
            if(string.IsNullOrEmpty(userId))
                throw new Exception("User not logged in");

            var orders = await _db.Orders
                            .Include(x => x.OrderStatus)
                            .Include(x=>x.OrderDetails)
                            .ThenInclude(x=>x.Product)
                            .ThenInclude(x=>x.Type)
                            .Where(a=>a.UserId==userId)
                            .ToListAsync();

            return orders;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

    }
}
