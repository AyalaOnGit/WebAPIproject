using Microsoft.EntityFrameworkCore;
using Repository;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class UserRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly db_shopContext _dbContext;
        private readonly UserRepository _userRepository;
        public UserRepositoryIntegrationTests(DatabaseFixture databaseFixture) {
            _dbContext = databaseFixture.Context;
            _userRepository = new UserRepository(_dbContext);
        }

        [Fact]
        public async Task GetUserById_WhenUserExists_ReturnsUser()
        {
            // Arrange
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            var testUser = new User { UserFirstName = "Bob",UserLastName="mom" ,UserEmail = "Bob@.com",Password = "Bob@.com!@" };
            await _dbContext.Users.AddAsync(testUser);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _userRepository.GetUserById(testUser.UserId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(testUser.UserId, result.UserId);
        }

        [Fact]
        public async Task GetUserById_WhenUserDoesNotExist_ReturnsNull()
        {
            // Arrange
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _userRepository.GetUserById(9999); // Assuming 9999 does not exist
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_ReturnsCorrectUser_WhenCredentialsMatch()
        {
            // Arrange
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            var user = new User { UserFirstName = "Charlie", UserLastName = "shwartz", UserEmail = "Charlie@.com", Password = "Charlie@.com!@" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _userRepository.Login("Charlie@.com", "Charlie@.com!@");
            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(user.UserFirstName, result.UserFirstName);
        }

        [Fact]
        public async Task Login_ReturnsNull_WhenCredentialsAreIncorrect()
        { 
            // Arrange
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            var testUser = new User { UserFirstName = "Alice",UserLastName="cohen", UserEmail = "Alice@.com", Password= "Alice@.com!@" };
            await _dbContext.Users.AddAsync(testUser);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _userRepository.Login("Alice@.com", "WrongPassword");
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddUser_AddsUserSuccessfully()
        {
            // Arrange
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            await _dbContext.SaveChangesAsync();
            var newUser = new User { UserFirstName = "David",UserLastName="levi", UserEmail = "David@.com", Password= "David@.com!@" };
            // Act
            var addedUser = await _userRepository.AddUser(newUser);
            // Assert
            Assert.NotNull(addedUser);
            Assert.Equal(newUser.UserFirstName, addedUser.UserFirstName);
            Assert.Equal(newUser.UserLastName, addedUser.UserLastName);
            Assert.Equal(newUser.UserEmail, addedUser.UserEmail);
            Assert.Equal(newUser.Password, addedUser.Password);
            var userInDb = await _dbContext.Users.FindAsync(addedUser.UserId);
            Assert.NotNull(userInDb);
            Assert.Equal(addedUser.UserId, userInDb.UserId);
        }

        //[Fact]
        //public async Task AddUser_DuplicateEmail_ThrowsException()
        //{
        //    // Arrange
        //    _dbContext.Users.RemoveRange(_dbContext.Users);
        //    var existingUser = new Entities.User { UserFirstName = "Eve", UserEmail = "Eve@.com" };
        //    await _dbContext.Users.AddAsync(existingUser);
        //    await _dbContext.SaveChangesAsync();
        //    var newUser = new Entities.User { UserFirstName = "EveDuplicate", UserEmail = "Eve@.com" };
        //    // Act & Assert
        //    await Assert.ThrowsAsync<DbUpdateException>(async () => await _userRepository.AddUser(newUser));
        //}

        [Fact]
        public async Task UpdateUser_UpdatesUserSuccessfully()
        {
            // Arrange
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            await _dbContext.SaveChangesAsync();
            var existingUser = new User { UserFirstName = "Frank", UserLastName = "perez", UserEmail = "Frank@.com", Password = "Frank@.com!@" };
            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            _dbContext.Entry(existingUser).State = EntityState.Detached;       
            var userToUpdate = new User
            {
                UserId = existingUser.UserId,
                UserFirstName = "After",
                UserLastName = "Update",
                UserEmail = "after@test.com",
                Password = "after@test.com!@"
            };
            // Act
            await _userRepository.UpdateUser(userToUpdate);
            // Assert
            _dbContext.ChangeTracker.Clear();
            Assert.NotNull(userToUpdate);
            var userInDb = await _dbContext.Users.FindAsync(userToUpdate.UserId);
            Assert.NotNull(userInDb);
            Assert.Equal("After", userInDb.UserFirstName);
            Assert.Equal("after@test.com", userInDb.UserEmail);
        }

        [Fact]
        public async Task UpdateUser_NonExistentUser_ThrowsException()
        {
            // Arrange
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            await _dbContext.SaveChangesAsync();
            var nonExistentUser = new Entities.User { UserId = 9999, UserFirstName = "Ghost", UserEmail = "ghost@.com" };
            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await _userRepository.UpdateUser(nonExistentUser));
        }
    }
 }
