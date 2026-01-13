namespace SklepWelniany.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Type> Types { get; set; }
        public string STerm { get; set; } = "";
        public int TypeId { get; set; } = 0;
    }
}
