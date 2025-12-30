using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repository;

namespace TestProject1
{
    public class UserRepositoryUnitTest
    {
        private List<User> GetTestUser()
        {
            return new List<User>
            {
                new User
                {
                    UserId = 1,
                    UserFirstName = "Alice",
                    UserLastName = "Smith",
                    UserEmail = "alice@test.com",
                    Password = "password123!@",
                    Orders = new List<Order> { new Order { OrderId = 1, OrderSum = 50.0 } }
                },
                new User
                {
                    UserId = 2,
                    UserFirstName = "Bob",
                    UserLastName = "Johnson",
                    UserEmail = "bob@test.com",
                    Password = "bob@test.com!@",
                    Orders = new List<Order>()
                }
            };
        }

        [Fact]
        public async Task GetUserById_UserExists_ReturnsUserWithOrders()
        {
            // Arrange
            var users = GetTestUser();
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.FindAsync<User>(1)).ReturnsAsync(users.First);
            var userRepository = new UserRepository(mockContext.Object);
            // Act
            var result = await userRepository.GetUserById(1);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
            Assert.Single(result.Orders);
        }

        [Fact]
        public async Task GetUserById_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var users = GetTestUser();
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);
            // Act
            var result = await userRepository.GetUserById(99);
            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, UserEmail = "alice@test.com", Password = "password123!@" },

                new User { UserId = 2, UserEmail = "alice@test.com", Password = "wrongpassword!@" }

            };
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);
            // Act
            var result = await userRepository.Login("alice@test.com", "password123!@");
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
        }
        [Fact]
        public async Task Login_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, UserEmail = "alice@test.com", Password = "password123!@" },
            };
            var mockContext = new Mock<db_shopContext>();

            mockContext.Setup(c => c.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);
            // Act
            var result = await userRepository.Login("alice@test.com", "wrongpassword");
            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task Login_EmailExists_WrongEmail_ReturnsNull()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, UserEmail = "alice@test.com", Password = "password123!@" }
            };
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);
            // Act
            var result = await userRepository.Login("alic@test.com", "password123!@");
            // Assert
            Assert.Null(result); // מחזיר null כי המייל לא תואם
        }

        [Fact]
        public async Task AddUser_ValidUser_ReturnsAddedUser()
        {
            // Arrange
            var users = new List<User>();
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);
            var newUser = new User
            {
                UserId = 1,
                UserFirstName = "Charlie",
                UserLastName = "Brown",
                UserEmail = "Charlie@test.com",
                Password = "charlie123!@"
            };
            // Act
            var result = await userRepository.AddUser(newUser);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
            mockContext.Verify(x => x.Users.AddAsync(It.IsAny<User>(), default), Times.Once);
            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);       
        }
        [Fact]
        public async Task UpdateUser_ExistingUser_UpdatesUser()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, UserFirstName = "David", UserLastName = "Smith", UserEmail =  "Charlie@test.com", Password = "charlie123!@", }
            };
            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);
            var updatedUser = new User
            {
                UserId = 1,
                UserFirstName = "David",
                UserLastName = "Johnson", // Updated last name
                UserEmail = "Charlie@test.com",
                Password = "charlie123!@",
            };
            // Act
            await userRepository.UpdateUser(updatedUser);
            // Assert
            //var userInDb = users.First(u => u.UserId == 1);
            //Assert.Equal("Johnson", userInDb.UserLastName);
            mockContext.Verify(x => x.Users.Update(It.Is<User>(u => u.UserLastName == "Johnson")), Times.Once); 
            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
        [Fact]
        public async Task UpdateUser_UserDoesNotExist_DoesNotThrow()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, UserFirstName = "David", UserLastName = "Smith", UserEmail =  "Charlie@test.com", Password = "charlie123!@", }
            };

            var mockContext = new Mock<db_shopContext>();
            mockContext.Setup(c => c.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);

            var updatedUser = new User
            {
                UserId = 99, // משתמש שלא קיים
                UserFirstName = "NonExistent",
                UserLastName = "User",
                UserEmail = "nouser@test.com"
            };
            // Act
            await userRepository.UpdateUser(updatedUser);  
            // Assertas
            Assert.DoesNotContain(users, u => u.UserId == 99); // המשתמש החדש לא נוסף
        }

    }
}