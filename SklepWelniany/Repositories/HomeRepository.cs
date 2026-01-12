namespace SklepWelniany.Repositories
{
    public class HomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Product>> GetProducts(string sTerm= "", int typeId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _db.Products
                            join type in _db.Types
                            on product.TypeId equals type.Id
                            where string.IsNullOrEmpty(sTerm) || (product != null && product.ProductName.ToLower().StartsWith(sTerm))

                            select new Product
                            {
                                Id = product.Id,
                                Image = product.Image,
                                Price = product.Price,
                                ProductName = product.ProductName,
                                TypeId = product.TypeId,
                                TypeName = type.ProductType
                            }
                            ).ToListAsync();

            // Filtrowanie po stronie klienta - nieoptymalne!!!!!
            if (typeId > 0)
            {
                products = products.Where(a=>a.TypeId == typeId).ToList();
            }
            return products;
        }
    }
}
