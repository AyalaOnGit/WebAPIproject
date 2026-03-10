namespace Services;
using AutoMapper;
using DTOs;
using Entities;
using Repository;
using System.ComponentModel.DataAnnotations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IPasswordService passwordService,IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _mapper = mapper;
    }

    public async Task<UserDTO> GetUserById(int id)
    {
        return _mapper.Map<User, UserDTO>(await _userRepository.GetUserById(id));
        
    }

    public async Task<ResultValidUser<UserDTO>> AddUser(UserDTO user,string password)
    {
        if (!IsValidEmail(user.UserEmail))
            return new ResultValidUser<UserDTO>(false, false,true, null);
        Password passwordAfterCheck = _passwordService.CheckPassword(password);
        if (passwordAfterCheck.Level < 3)
            return new ResultValidUser<UserDTO>(true, false,false, null);
        if (await EmailExists(user.UserEmail, user.UserId))
            return new ResultValidUser<UserDTO>(false, true,false, null);
        User user1 = _mapper.Map<UserDTO, User>(user);
        user1.Password = password;
        UserDTO user2= _mapper.Map<User, UserDTO>(await _userRepository.AddUser(user1));
        ResultValidUser<UserDTO> resultValidUser= new ResultValidUser<UserDTO>(false, false,false, user2);
        return resultValidUser;
    }
    public async Task<ResultValidUser<bool>> UpdateUser(int id, UserDTO user,string password)
    {
        if (!IsValidEmail(user.UserEmail))
            return new ResultValidUser<bool>(false, false, true, false);
        Password passwordAfterCheck = _passwordService.CheckPassword(password);
        if (passwordAfterCheck.Level < 3)
        {
            return new ResultValidUser<bool>(true, false,false, false);
        }
        else if (await EmailExists(user.UserEmail, id))
        {
            return new ResultValidUser<bool>(false, true,false, false);
        }
        else
        {
            User user1 = _mapper.Map<UserDTO, User>(user);
            user1.UserId = id;
            user1.Password = password;
            await _userRepository.UpdateUser(user1);
            return new ResultValidUser<bool>(false, false, false, true);
        }
    }
    public async Task<UserDTO> Login(LoginUserDTO loginUser)
    {
        return _mapper.Map<User, UserDTO>(await _userRepository.Login(loginUser.UserEmail, loginUser.UserPassword));
    }
    
    public async Task<bool> EmailExists(string email,int id)
    {
        User user = await _userRepository.GetUserByEmail(email);
        if (user != null && user.UserId!=id)
        {
            return true;
        }
        return false;
    }

    public bool IsValidEmail(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }
}
