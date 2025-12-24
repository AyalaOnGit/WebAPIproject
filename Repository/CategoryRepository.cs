using System.Text.Json;
using Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        db_shopContext _ShopContext;
        public CategoryRepository(db_shopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }
        public async Task<List<Category>> GetCategories()
        {
            return await _ShopContext.Categories.ToListAsync();
        }

    
    }
}
