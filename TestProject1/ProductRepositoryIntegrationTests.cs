using Repository;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class ProductRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly db_shopContext _dbContext;
        private readonly ProductRepository _productRepository;
        public ProductRepositoryIntegrationTests(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _productRepository = new ProductRepository(_dbContext);
        }

        [Fact]
        public async Task GetProducts_WhenDataExists_ReturnsAllProductsWithCategory()
        {
            // Arrange
            _dbContext.Products.RemoveRange(_dbContext.Products);
            _dbContext.Categories.RemoveRange(_dbContext.Categories);
            await _dbContext.SaveChangesAsync();
            var category = new Category { CategoryName = "TestCategory" };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            var testProducts = new List<Product>
            {
                new Product { ProductName = "Product1", Price = 10, CategoryId = category.CategoryId, Description = "Desc1" },
                new Product { ProductName = "Product2", Price = 20, CategoryId = category.CategoryId, Description = "Desc2" }
            };
            await _dbContext.Products.AddRangeAsync(testProducts);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _productRepository.GetProducts(null, null, null, null, null, null, null);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(testProducts.Count, result.Count);
            Assert.All(result, p => Assert.NotNull(p.Category));
            foreach (var product in testProducts)
            {
                Assert.Contains(result, p => p.ProductName == product.ProductName && p.Category != null);
            }
        }

        [Fact]
        public async Task GetProducts_ReturnsEmpty_WhenNoDataExists()
        {
            // Arrange
            _dbContext.Products.RemoveRange(_dbContext.Products);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _productRepository.GetProducts(null, null, null, null, null, null, null);
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }   
    }
}
