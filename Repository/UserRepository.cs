using System.Text.Json;
using Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        db_shopContext _ShopContext;
        public UserRepository(db_shopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }

        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        public async Task<User> GetUserById(int id)
        {
            return await _ShopContext.FindAsync<User>(id);
        }


        public async Task<User> AddUser(User user)
        {
            await _ShopContext.Users.AddAsync(user);
            await _ShopContext.SaveChangesAsync(); 
            return await _ShopContext.Users.FindAsync(user.UserId);
        }



        public async Task<User> Login(string email, string passsword)
        {
            return await _ShopContext.Users.FirstOrDefaultAsync(x => x.UserEmail == email && x.Password == passsword);    
        }


        public async Task UpdateUser(User updatedUser)
        {
            _ShopContext.Users.Update(updatedUser);
            await _ShopContext.SaveChangesAsync();
        }


        public void DeleteUser(int id)
        {
        }
    }
}
