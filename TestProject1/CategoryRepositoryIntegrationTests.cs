using Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class CategoryRepositoryIntegrationTests : IDisposable
    {
        private readonly db_shopContext _dbContext;
        private readonly CategoryRepository _categoryRepository;
        private readonly DatabaseFixture _fixture;
        public CategoryRepositoryIntegrationTests()
        {
            _fixture = new DatabaseFixture();
            _dbContext = _fixture.Context;
            _categoryRepository = new CategoryRepository(_dbContext);
        }
        public void Dispose() 
        {
            _fixture.Dispose();
        }
        
        [Fact]
        public async Task GetCategories_WhenDataExists_ReturnsAllCategories()
        {
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
            // Act
            var result = await _categoryRepository.GetCategories();
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
