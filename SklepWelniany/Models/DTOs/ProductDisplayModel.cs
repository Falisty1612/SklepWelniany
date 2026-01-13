namespace SklepWelniany.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Type> Types { get; set; }
    }
}
