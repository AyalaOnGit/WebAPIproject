using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repository;
namespace TestProject1
{
    public class CategoryRepositoryUnitTest
    {
        [Fact]
        public async Task GetCategory_ValidCredentials_ReturnsCategory()
        {
            // Arrange
            var category1 = new Category { CategoryName="toys"};
            var category2 = new Category { CategoryName = "books" };

            var mockContext = new Mock<db_shopContext>();
            var categories = new List<Category>() { category1 , category2 };
            mockContext.Setup(x => x.Categories).ReturnsDbSet(categories);

            var categoryRepository = new CategoryRepository(mockContext.Object);

            // Act
            var result = await categoryRepository.GetCategories();

            // Assert
            Assert.Equal(categories, result);
        }

        [Fact]
        public async Task GetCategories_WhenEmpty_ReturnsNoItems()
        {
            // Arrange
            var categories = new List<Category>(); // Empty list

            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(x => x.Categories).ReturnsDbSet(categories);

            var categoryRepository = new CategoryRepository(mockContext.Object);

            // Act
            var result = await categoryRepository.GetCategories();

            // Assert
            Assert.Empty(result);     // אין פריטים = NOITEM
            Assert.NotNull(result);  // לא null          
        }
    }
}
