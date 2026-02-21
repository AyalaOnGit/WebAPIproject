using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Entities;

namespace Services
{
    public class RatingService: IRatingService
    {
        private readonly IRatingRepository _RatingRepository;   
        public RatingService(IRatingRepository RatingRepository)
        {
            _RatingRepository = RatingRepository;
        }
        public async Task<Rating> AddRating(Rating rating)
        {
            return await _RatingRepository.AddRating(rating);
        }

    }
}
