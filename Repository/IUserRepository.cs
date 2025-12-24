using Entities;

namespace Repository
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user);
        void DeleteUser(int id);
        Task<User> GetUserById(int id);
        Task<User> Login(string email, string passsword);
        Task UpdateUser(User updatedUser);
    }
}