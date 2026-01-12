namespace SklepWelniany.Repositories
{
    public class HomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Product>> DisplayProducts(string sTerm="", int typeId=0)
        {
            var products = (from product in _db.Products
                            join type in _db.Types
                            on product.TypeId equals type.Id
                            select new Product
                            {
                                Id = product.Id,
                                Image = product.Image,
                                Price = product.Price,
                                ProductName = product.ProductName,
                                TypeId = product.TypeId,
                                TypeName = type.TypeName
                            }
                            ).ToListAsync();
        }
    }
}
