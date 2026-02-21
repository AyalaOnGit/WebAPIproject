using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class RatingRepositoryUnitTest
    {
        [Fact]
        public async Task AddRating_ReturnsRating_WhenCalled()
        {
            // Arrange
            var newRating = new Rating
            {
                Host = "localhost",
                Method = "POST",
                Path = "/api/User/login",
                RecordDate = DateTime.Now
            };

            var mockContext = new Mock<db_shopContext>();

            mockContext.Setup(x => x.Ratings.AddAsync(It.IsAny<Rating>(), It.IsAny<CancellationToken>()))
                       .Returns(new ValueTask<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Rating>>(null as Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Rating>));

            var repository = new RatingRepository(mockContext.Object);

            // Act
            var result = await repository.AddRating(newRating);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("POST", result.Method);
            Assert.Equal("localhost", result.Host);

            mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task AddRating_ValidRating_ReturnsAddedRating()
        {
            // Arrange
            var newRating = new Rating { Host = "localhost", Method = "GET", Path = "/api/products" };
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(x => x.Ratings).ReturnsDbSet(new List<Rating>());
            var ratingRepository = new RatingRepository(mockContext.Object);
            // Act
            var result = await ratingRepository.AddRating(newRating);
            // Assert
            Assert.Equal(newRating, result);
            mockContext.Verify(x => x.Ratings.AddAsync(newRating, default), Times.Once);
            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
        [Fact]
        public async Task AddRating_DatabaseError_ThrowsException()
        {
            // Arrange
            var newRating = new Rating { Host = "localhost", Method = "GET", Path = "/api/products" };
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(x => x.Ratings).ReturnsDbSet(new List<Rating>());
            mockContext.Setup(x => x.SaveChangesAsync(default)).ThrowsAsync(new Exception("Database error"));
            var ratingRepository = new RatingRepository(mockContext.Object);
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => ratingRepository.AddRating(newRating));
        }
        [Fact]

        public async Task AddRating_SavesRatingToDatabase()
        {
            // Arrange
            var newRating = new Rating { Host = "localhost", Method = "GET", Path = "/api/products" };
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(x => x.Ratings).ReturnsDbSet(new List<Rating>());
            var ratingRepository = new RatingRepository(mockContext.Object);
            // Act
            await ratingRepository.AddRating(newRating);
            // Assert
            mockContext.Verify(x => x.Ratings.AddAsync(newRating, default), Times.Once);
            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
        [Fact]
        public async Task AddRating_ValidRating_ReturnsRatingWithId()
        {
            // Arrange
            var newRating = new Rating { Host = "localhost", Method = "GET", Path = "/api/products" };
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(x => x.Ratings).ReturnsDbSet(new List<Rating>());
            mockContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1).Callback(() =>
            {
                newRating.RatingId = 1; // Simulate database assigning an ID
            });
            var ratingRepository = new RatingRepository(mockContext.Object);
            // Act
            var result = await ratingRepository.AddRating(newRating);
            // Assert
            Assert.Equal(1, result.RatingId);
        }
    }
}
