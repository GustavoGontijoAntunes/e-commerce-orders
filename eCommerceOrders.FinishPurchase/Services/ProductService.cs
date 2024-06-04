using eCommerceOrders.FinishPurchase.Models;
using eCommerceOrders.FinishPurchase.Persistence;
using eCommerceOrders.FinishPurchase.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eCommerceOrders.FinishPurchase.Services
{
    public class ProductService : IProductService
    {
        private readonly ECommerceContext _context;

        public ProductService(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Product.FindAsync(id);
        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<int> ids)
        {
            return await _context.Product.Where(p => ids.Contains(p.Id)).ToListAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}