namespace SklepWelniany
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product>> GetProducts(string sTerm = "", int typeId = 0);
        Task<IEnumerable<Models.Type>> Types();
    }
}