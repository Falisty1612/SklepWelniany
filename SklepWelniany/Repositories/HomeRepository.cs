namespace SklepWelniany.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Models.Type>> Types()
        {
            return await _db.Types.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts(string sTerm= "", int typeId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _db.Products
                            join type in _db.Types
                            on product.TypeId equals type.Id

                            join stock in _db.Stocks
                            on product.Id equals stock.ProductId
                            into product_stocks
                            from productWithStock in product_stocks.DefaultIfEmpty()

                            where 
                                (string.IsNullOrWhiteSpace(sTerm) || (product != null && product.ProductName.ToLower().Contains(sTerm)))
                                && (typeId == 0 || product.TypeId == typeId)
                            select new Product
                            {
                                Id = product.Id,
                                Image = product.Image,
                                Price = product.Price,
                                ProductName = product.ProductName,
                                TypeId = product.TypeId,
                                TypeName = type.ProductType,
                                Quantity = productWithStock == null ? 0 : productWithStock.Quantity
                            }
                            ).ToListAsync();


            return products;
        }
    }
}
