using Microsoft.AspNetCore.Mvc;
using Services;
using Repository;
using DTOs;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id) 
        {
            UserDTO user = await _userService.GetUserById(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }
  
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserDTO user, string password)
        {
            ResultValidUser<UserDTO> createdUser = await _userService.AddUser(user, password);
            if (createdUser.data!=null)
                return CreatedAtAction(nameof(Get), new { id = createdUser.data.UserId }, createdUser.data);
            if (createdUser.InvalidPassword)
                return BadRequest("Password is not strong enough");
            return BadRequest("Email already exists");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginUserDTO loginUser)
        {
            if (loginUser == null || string.IsNullOrWhiteSpace(loginUser.UserEmail))
                return BadRequest("Email is required");
            UserDTO user = await _userService.Login(loginUser);
            if (user != null)
            {
                _logger.LogInformation("Login attempted with User Name, {0} and password {1}", loginUser.UserEmail, loginUser.UserPassword);
                return Ok(user);
            }
            return Unauthorized("Invalid email or password");
        }
       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDTO user, string password)
        {
            ResultValidUser<bool> isUpdateSuccessfulResult = await _userService.UpdateUser(id, user, password);
            bool isUpdateSuccessful = isUpdateSuccessfulResult.data;
            if (isUpdateSuccessful)
            {
                 return Ok();
            }
            if (isUpdateSuccessfulResult.UserAlreadyExists)
                return BadRequest("Email already exists");
            return BadRequest("Password is not strong enough");
        }
    }
}
