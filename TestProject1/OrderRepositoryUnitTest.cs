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
    public class OrderRepositoryUnitTest
    {
        [Fact]
        public async Task GetOrderById_OrderExists_ReturnsOrderWithItems()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    UserId = 1,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    OrderSum = 100,
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { OrderItemId = 1, ProductId = 2 , Quantity= 1,}
                    }

                }
            };

            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.Orders).ReturnsDbSet(orders);
            var orderrepository = new OrderRepository(mockContext.Object);

            // Act
            var result = await orderrepository.GetOrderById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.OrderId);
            Assert.Single(result.OrderItems);
        }

        [Fact]
        public async Task GetOrderById_OrderDoesNotExist_ReturnsNull()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    UserId = 1,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    OrderSum = 100,
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { OrderItemId = 1, ProductId = 2 , Quantity= 1,}
                    }
                }
            };

            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.Orders).ReturnsDbSet(orders);
            var orderrepository = new OrderRepository(mockContext.Object);

            // Act
            var result = await orderrepository.GetOrderById(2); // Non-existing order ID

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddOrder_ValidOrder_ReturnsAddedOrder()
        {
            // Arrange
            var newOrder = new Order
            {
                OrderId = 1,
                UserId = 2,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = 200,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { OrderItemId = 2, ProductId = 3 , Quantity= 2,}
                }
            };

            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.Orders).ReturnsDbSet(new List<Order>());
            var orderrepository = new OrderRepository(mockContext.Object);

            // Act
            var result = await orderrepository.AddOrder(newOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newOrder.OrderId, result.OrderId);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
}
