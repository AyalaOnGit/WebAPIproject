using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RatingRepository : IRatingRepository
    {
        db_shopContext _ShopContext;
        public RatingRepository(db_shopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }
        
        public async Task<Rating> AddRating(Rating rating)
        {
            await _ShopContext.Ratings.AddAsync(rating);
            await _ShopContext.SaveChangesAsync();
            return rating;
        }
    }
}
