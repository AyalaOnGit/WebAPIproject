using Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    [Collection("Database collection")]
    public class CategoryRepositoryIntegrationTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
    {
        private readonly db_shopContext _dbContext;
        private readonly CategoryRepository _categoryRepository;
        private readonly DatabaseFixture _fixture;

        public CategoryRepositoryIntegrationTests(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _categoryRepository = new CategoryRepository(_dbContext);
            _fixture = databaseFixture;
        }
        public async Task InitializeAsync()
        {
            // מחיקת רשומות בכל הטבלאות לפי סדר תלות (Foreign Keys)
            _dbContext.OrderItems.RemoveRange(_dbContext.OrderItems);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            _dbContext.Products.RemoveRange(_dbContext.Products);
            _dbContext.Categories.RemoveRange(_dbContext.Categories);
            _dbContext.Users.RemoveRange(_dbContext.Users);

            // שמירת השינויים
            await _dbContext.SaveChangesAsync();
        }
        public Task DisposeAsync()
        {
            // כאן הקוד שרץ אחרי כל טסט (TearDown)
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetCategories_WhenDataExists_ReturnsAllCategories()
        {
            // Arrange
            //_dbContext.Categories.RemoveRange(_dbContext.Categories);
            var testCategories = new List<Category>
            {
                new Category { CategoryName = "Electronics" },
                new Category { CategoryName = "Books" },
                new Category { CategoryName = "Clothing" }
            };
            await _dbContext.Categories.AddRangeAsync(testCategories);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _categoryRepository.GetCategories();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(testCategories.Count, result.Count());
            foreach (var category in testCategories)
            {
                Assert.Contains(result, c => c.CategoryName == category.CategoryName);
            }
        }

        [Fact]
        public async Task GetCategories_ReturnsEmpty_WhenNoDataExists()
        {             
            // Arrange
            //_dbContext.Categories.RemoveRange(_dbContext.Categories);
            //await _dbContext.SaveChangesAsync();
            // Act
            var result = await _categoryRepository.GetCategories();
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
