namespace Services;
using Entities;
using DTOs;
using Repository;
using AutoMapper;

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
        Password passwordAfterCheck = _passwordService.CheckPassword(password);
        if (passwordAfterCheck.Level < 3)
            return new ResultValidUser<UserDTO>(true, false, null);
        if (EmailExists(user.UserEmail, user.UserId).Result)
            return new ResultValidUser<UserDTO>(false, true, null);
        User user1 = _mapper.Map<UserDTO, User>(user);
        user1.Password = password;
        UserDTO user2= _mapper.Map<User, UserDTO>(await _userRepository.AddUser(user1));
        ResultValidUser<UserDTO> resultValidUser= new ResultValidUser<UserDTO>(false, false, user2);
        return resultValidUser;
    }
    public async Task<ResultValidUser<bool>> UpdateUser(int id, UserDTO user,string password)
    {
        Password passwordAfterCheck = _passwordService.CheckPassword(password);
        if (passwordAfterCheck.Level < 3)
        {
            return new ResultValidUser<bool>(true, false, false);
        }
        else if (EmailExists(user.UserEmail, id).Result)
        {
            return new ResultValidUser<bool>(false, true, false);
        }
        else
        {
            User user1 = _mapper.Map<UserDTO, User>(user);
            user1.UserId = id;
            user1.Password = password;
            await _userRepository.UpdateUser(user1);
            return new ResultValidUser<bool>(false, false, true);
        }
    }
    public async Task<UserDTO> Login(LoginUserDTO loginUser)
    {
        return _mapper.Map<User, UserDTO>(await _userRepository.Login(loginUser.UserEmail, loginUser.UserPassword));
    }
    
    public async Task<bool> EmailExists(string email,int id)
    {
        User user = await _userRepository.GetUserByEmail(email);
        if (user.UserId!=id && user != null)
        {
            return true;
        }
        return false;
    }

}
