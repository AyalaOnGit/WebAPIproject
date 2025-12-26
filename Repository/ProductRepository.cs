using System.Text.Json;
using Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        db_shopContext _ShopContext;
        public ProductRepository(db_shopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }

        public async Task<List<Product>> GetProducts(string? name, int[]? categories, int? nimPrice, int? maxPrice, int? limit, string? orderBy, int? offset)
        {
            return await _ShopContext.Products.Include(p=>p.Category).ToListAsync();
        }
        
    }
}
