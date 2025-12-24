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

    public async Task<UserDTO> AddUser(UserDTO user,string password)
    {
        Password passwordAfterCheck = _passwordService.CheckPassword(password);
        if (passwordAfterCheck.Level < 3)
            return null;
        return _mapper.Map<User, UserDTO>(await _userRepository.AddUser(_mapper.Map <  UserDTO,User > (user)));
    }
    public async Task<bool> UpdateUser(int id, UserDTO user,string password)
    {
        Password passwordAfterCheck = _passwordService.CheckPassword(password);
        if (passwordAfterCheck.Level < 3)
        {
            return false;
        }
        else
        {
            await _userRepository.UpdateUser(id, _mapper.Map < UserDTO, User > (user));
            return true;
        }
    }
    public async Task<UserDTO> Login(LoginUserDTO loginUser)
    {
        return _mapper.Map<User, UserDTO>(await _userRepository.Login(loginUser.UserEmail,loginUser.UserPassword));
    }
    //public void DeleteUser(int id)
    //{
    //    _userRepository.DeleteUser(id);
    //}
}
