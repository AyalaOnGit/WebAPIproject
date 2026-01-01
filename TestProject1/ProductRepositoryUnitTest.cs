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
    public class ProductRepositoryUnitTest
    {
        private List<Product> GetTestProducts()
        {
            var category = new Category { CategoryId = 1, CategoryName = "TestCategory" };
            return new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Product1", Price = 10, Category = category, CategoryId = 1, Description = "Desc1" },
                new Product { ProductId = 2, ProductName = "Product2", Price = 20, Category = category, CategoryId = 1, Description = "Desc2" }
            };
        }

        [Fact]
        public async Task GetProducts_ReturnsAllProductsWithCategory()
        {
            // Arrange
            var products = GetTestProducts();
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.Products).ReturnsDbSet(products);
            var productRepository = new ProductRepository(mockContext.Object);

            // Act
            var result = await productRepository.GetProducts(null, null, null, null, null, null, null);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.All(result.Items, p => Assert.NotNull(p.Category));
        }

        [Fact]
        public async Task GetProducts_NoProducts_ReturnsEmptyList()
        {
            // Arrange
            var products = new List<Product>();
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.Products).ReturnsDbSet(products);
            var productRepository = new ProductRepository(mockContext.Object);

            // Act
            var result = await productRepository.GetProducts(null, null, null, null, null, null, null);

            // Assert
            Assert.Empty(result.Items);
        }
    }
}
