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

        public async Task<(List<Product> Items,int TotalCount)> GetProducts(string? description, int[]? categories, int? minPrice, int? maxPrice, int? skip, string? orderBy, int? position)
        {
            var query = _ShopContext.Products.Where(product =>
                        (description == null ? (true) : (product.Description.Contains(description)))
                        && ((minPrice == null) ? (true) : (product.Price >= minPrice))
                        && ((maxPrice == null) ? (true) : (product.Price <= maxPrice))
                        && ((categories.Length==0) ? (true) : (categories.Contains(product.CategoryId))))
                        .OrderBy(product => product.Price);
            var position1 = position ?? 1;
            var skip1 = skip ?? 10;
            Console.WriteLine(query.ToQueryString());
            List<Product> products = await query.Skip((position1 - 1) * skip1)
                                    .Take(skip1).Include(product => product.Category).ToListAsync();
            var total = await query.CountAsync();
            return (products, total);

        }

    }
}
