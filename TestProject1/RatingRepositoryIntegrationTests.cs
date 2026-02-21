using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class RatingRepositoryIntegrationTests : IDisposable
    {
        private readonly db_shopContext _dbContext;
        private readonly RatingRepository _ratingRepository;
        private readonly DatabaseFixture _fixture;
        public RatingRepositoryIntegrationTests()
        {
            _fixture = new DatabaseFixture();
            _dbContext = _fixture.Context;
            _ratingRepository = new RatingRepository(_dbContext);
        }
        public void Dispose()
        {
            _fixture.Dispose();
        }

        [Fact]
        public async Task AddRating_Integration_SavesToDatabase()
        {
            // Arrange
            var rating = new Rating
            {
                Host = "127.0.0.1",
                Method = "GET",
                Path = "/api/Products",
                UserAgent = "Mozilla/5.0",
                Referer = "https://google.com"
            };

            // Act
            var result = await _ratingRepository.AddRating(rating);

            // Assert
            Assert.NotEqual(0, result.RatingId);

            _dbContext.ChangeTracker.Clear();
            var saved = await _dbContext.Ratings.FindAsync(result.RatingId);

            // Assert
            Assert.NotNull(saved);
            Assert.Equal("GET", saved.Method.Trim());
            Assert.Equal("127.0.0.1", saved.Host.Trim());
        }

        [Fact]
        public async Task AddRating_IntegrationTest_ReturnsAddedRating()
        {
            // Arrange
            var newRating = new Rating { Host = "localhost", Method = "GET", Path = "/api/products", UserAgent = "Mozilla/5.0", Referer = "https://google.com" };
            // Act
            var result = await _ratingRepository.AddRating(newRating);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(newRating.Host, result.Host);
            Assert.Equal(newRating.Method, result.Method);
            Assert.Equal(newRating.Path, result.Path);
            Assert.Equal(newRating.Referer, result.Referer);
            Assert.Equal(newRating.UserAgent, result.UserAgent);
        }
        [Fact]
        public async Task AddRating_IntegrationTest_SavesToDatabase()
        {
            // Arrange
            var newRating = new Rating { Host = "localhost", Method = "POST", Path = "/api/user/login", UserAgent = "Mozilla/5.0", Referer = "https://google.com" };
            // Act
            var result = await _ratingRepository.AddRating(newRating);
            var savedRating = await _dbContext.Ratings.FindAsync(result.RatingId);
            // Assert
            Assert.NotNull(savedRating);
            Assert.Equal(newRating.Host, savedRating.Host);
            Assert.Equal(newRating.Method, savedRating.Method);
            Assert.Equal(newRating.Path, savedRating.Path);
            Assert.Equal(newRating.Referer, savedRating.Referer);
            Assert.Equal(newRating.UserAgent, savedRating.UserAgent);
        }
        [Fact]
        public async Task AddRating_IntegrationTest_InvalidData_ThrowsException()
        {
            // Arrange
            var invalidRating = new Rating { Host = null, Method = "GET", Path = "/api/products" }; // Host is required
            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await _ratingRepository.AddRating(invalidRating));
        }
        [Fact]
        public async Task AddRating_IntegrationTest_MultipleRatings_ReturnsAddedRatings()
        {
            // Arrange
            var rating1 = new Rating { Host = "localhost", Method = "GET", Path = "/api/products", UserAgent = "Mozilla/5.0", Referer = "https://google.com" };
            var rating2 = new Rating { Host = "localhost", Method = "POST", Path = "/api/user/login", UserAgent = "Mozilla/5.0", Referer = "https://google.com" };
            // Act
            var result1 = await _ratingRepository.AddRating(rating1);
            var result2 = await _ratingRepository.AddRating(rating2);
            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(rating1.Host, result1.Host);
            Assert.Equal(rating1.Method, result1.Method);
            Assert.Equal(rating1.Path, result1.Path);
            Assert.Equal(rating1.UserAgent, result1.UserAgent);
            Assert.Equal(rating1.UserAgent, result1.UserAgent);
            Assert.Equal(rating2.Referer, result2.Referer);
            Assert.Equal(rating2.UserAgent, result2.UserAgent);
            Assert.Equal(rating2.Host, result2.Host);
            Assert.Equal(rating2.Method, result2.Method);
            Assert.Equal(rating2.Path, result2.Path);
        }
    }
}