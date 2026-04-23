namespace Services;
using AutoMapper;
using DTOs;
using Entities;
using Repository;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IPasswordService passwordService, IMapper mapper, IDistributedCache cache, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _mapper = mapper;
        _cache = cache;
        _configuration = configuration;
    }

    public async Task<UserDTO> GetUserById(int id)
    {
        string cacheKey = $"user_{id}";
        var cachedUser = await _cache.GetStringAsync(cacheKey);
        if (cachedUser != null)
        {
            return JsonSerializer.Deserialize<UserDTO>(cachedUser);
        }

        var user = _mapper.Map<User, UserDTO>(await _userRepository.GetUserById(id));
        await SetCacheAsync(cacheKey, user);
        return user;
    }

    public async Task<ResultValidUser<UserDTO>> AddUser(UserWithPasswordDTO user)
    {
        if (!IsValidEmail(user.UserEmail))
            return new ResultValidUser<UserDTO>(false, false,true, null);
        Password passwordAfterCheck = _passwordService.CheckPassword(user.UserPassword);
        if (passwordAfterCheck.Level < 3)
            return new ResultValidUser<UserDTO>(true, false,false, null);
        if (await EmailExists(user.UserEmail, user.UserId))
            return new ResultValidUser<UserDTO>(false, true,false, null);
        User user1 = _mapper.Map<UserWithPasswordDTO, User>(user);
        user1.Password = user.UserPassword;
        UserDTO user2= _mapper.Map<User, UserDTO>(await _userRepository.AddUser(user1));
        await InvalidateUserCache(user2.UserId);
        ResultValidUser<UserDTO> resultValidUser= new ResultValidUser<UserDTO>(false, false,false, user2);
        return resultValidUser;
    }
    public async Task<ResultValidUser<bool>> UpdateUser(int id, UserWithPasswordDTO user)
    {
        if (!IsValidEmail(user.UserEmail))
            return new ResultValidUser<bool>(false, false, true, false);
        Password passwordAfterCheck = _passwordService.CheckPassword(user.UserPassword);
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
            User user1 = _mapper.Map<UserWithPasswordDTO, User>(user);
            user1.UserId = id;
            user1.Password = user.UserPassword;
            await _userRepository.UpdateUser(user1);
            await InvalidateUserCache(id);
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

    public async Task InvalidateUserCache(int userId)
    {
        string cacheKey = $"user_{userId}";
        await _cache.RemoveAsync(cacheKey);
    }

    private async Task SetCacheAsync(string cacheKey, UserDTO user)
    {
        var ttlString = _configuration["Redis:TTL"];
        var ttl = string.IsNullOrEmpty(ttlString) ? 3600 : int.Parse(ttlString);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ttl)
        };
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(user), options);
    }
}
