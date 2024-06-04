using eCommerceOrders.FinishPurchase.Models;

namespace eCommerceOrders.FinishPurchase.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsByIdsAsync(List<int> ids);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
    }
}