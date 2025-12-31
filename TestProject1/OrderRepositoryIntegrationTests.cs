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
    public class OrderRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly db_shopContext _dbContext;
        private readonly OrderRepository _orderRepository;
        public OrderRepositoryIntegrationTests(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _orderRepository = new OrderRepository(_dbContext);
        }

        [Fact]
        public async Task GetOrders_WhenDataExists_ReturnsAllOrders()
        {
            // Arrange
            _dbContext.OrderItems.RemoveRange(_dbContext.OrderItems);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            _dbContext.Products.RemoveRange(_dbContext.Products);
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.Categories.RemoveRange(_dbContext.Categories);
            await _dbContext.SaveChangesAsync();
            var user = new User { UserFirstName = "TestUser", UserEmail = "TestUser@.com", Password = "password!@#" };
            var category = new Category { CategoryName = "General" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            var product = new Product { ProductName = "TestProduct", Price = 50, Category = category };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            var testOrders = new Order
            {
                UserId = user.UserId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = 100,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = product.ProductId, Quantity = 2 }
                }
            };
            await _dbContext.Orders.AddRangeAsync(testOrders);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _orderRepository.GetOrderById(testOrders.OrderId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(testOrders.OrderId, result.OrderId);
        }

        [Fact]
        public async Task GetOrders_ReturnsNull_WhenNoDataExists()
        {
            // Arrange
            _dbContext.OrderItems.RemoveRange(_dbContext.OrderItems);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _orderRepository.GetOrderById(9999); // Assuming 9999 is a non-existent OrderId
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddOrder_PersistsOrderAndItemsToDatabase()
        {
            // Arrange
            _dbContext.OrderItems.RemoveRange(_dbContext.OrderItems);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            _dbContext.Products.RemoveRange(_dbContext.Products);
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.Categories.RemoveRange(_dbContext.Categories);
            await _dbContext.SaveChangesAsync();
            var user = new User { UserFirstName = "TestUser", UserEmail = "TestUser@.com", Password = "password!@#" };
            var category = new Category { CategoryName = "General" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            var product = new Product { ProductName = "TestProduct", Price = 50, Category = category };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            var newOrder = new Order
            {
                UserId = user.UserId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = 100,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = product.ProductId, Quantity = 2 }
                }
            };
            // Act           
            var savedOrder = await _orderRepository.AddOrder(newOrder);
            // Assert
            Assert.NotEqual(0, savedOrder.OrderId);
            Assert.NotNull(savedOrder);
            Assert.Equal(newOrder.OrderId, savedOrder.OrderId);
            Assert.Equal(1, savedOrder.OrderItems.Count);
            Assert.Equal(product.ProductId, savedOrder.OrderItems.First().ProductId);
            Assert.Equal(2, savedOrder.OrderItems.First().Quantity);
            var dbOrder = await _dbContext.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.OrderId == savedOrder.OrderId);
        }  
    }
}
