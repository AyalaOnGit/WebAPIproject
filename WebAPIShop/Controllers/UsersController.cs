using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Services;
using Repository;
using DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIShop.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id) 
        {
            UserDTO user = await _userService.GetUserById(id);
            if(user!= null)
            {
                return Ok(user);
            }
            return NotFound();
        }
  
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserDTO user,string password)
        {
            UserDTO createdUser = await _userService.AddUser(user,password);
            if(createdUser!=null)
                return CreatedAtAction(nameof(Get), new{id = createdUser.id}, createdUser);
            return BadRequest("Password is not strong enough");
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginUserDTO loginUser)
        {
            if (loginUser == null || string.IsNullOrWhiteSpace(loginUser.UserEmail))
                return BadRequest("Email is required");
            UserDTO user = await _userService.Login(loginUser);
            if (user != null)
            {
                return Ok(user);
            }
            //return NoContent();
            return Unauthorized("Invalid email or password");
        }
       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDTO user,string password)
        {
            bool isUpdateSuccessful = await _userService.UpdateUser(id, user,password);
            if (!isUpdateSuccessful)
            {
                return BadRequest("Password is not strong enough");
            }
            return NoContent();
        }

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    _userService.DeleteUser(id);
        //}
    }
}
