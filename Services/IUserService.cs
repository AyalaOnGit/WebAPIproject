using DTOs;
using Entities;
using Repository;
using AutoMapper;

namespace Services
{
    public interface IUserService
    {
        Task<ResultValidUser<UserDTO>> AddUser(UserDTO user,string password);
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> Login(LoginUserDTO loginUser);
        Task<ResultValidUser<bool>> UpdateUser(int id, UserDTO user,string password);
        Task<bool> EmailExists(string email,int id);
        bool IsValidEmail(string email);
    }
}