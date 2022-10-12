using FacebookAPI.App.Models.ApiModels;
using FacebookAPI.App.Models.PostModels;
using FacebookAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerHelper
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<List<ApiUser>>> GetUsers()
        {
            var users = await _service.GetUsersAsync();

            if (users.Count > 0)
            {
                return Ok(users);
            }

            return NotFound("No users found");
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiUser>> GetUser(int id)
        {
            if (!await _service.UserExistsAsync(id))
            {
                return NotFound(UserDoesNotExist(id));
            }

            var user = await _service.GetUserAsync(id);

            return Ok(ApiMapper.MapUser(user));
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiUser>> PutUser(int id, ApiUser user)
        {          
            if (!await _service.UserExistsAsync(id))
            {
                return NotFound(UserDoesNotExist(id));
            }

            var coreUser = ApiMapper.MapUser(user);

            if (await _service.UpdateUserAsync(id, coreUser))
            {
                return ApiMapper.MapUser(coreUser);
            }

            return StatusCode(500, UserErrorMessage());
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult> PostUser(RegisterModel user)
        {
            if (!user.PasswordsAreTheSame())
            {
                return BadRequest("Password and Confirm Pasword must be the same");
            }

            if (!_service.PasswordMeetsRequirements(user.Password))
            {
                return BadRequest("Password does not meet requirements");
            }

            if (user.IsAdmin)
            {
                return BadRequest("You cannot register as an admin");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            if (await _service.AddUserAsync(ApiMapper.MapUser(user)))
            {
                return Ok("User has been registered");
            }

            return StatusCode(500, UserErrorMessage());
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _service.DeleteUserAsync(id))
            {
                return Ok("User has been deleted");
            } else
            {
                return StatusCode(500, "An error has occured");
            }
        }

        private string UserDoesNotExist(int id)
        {
            return $"User with an id of {id} does not exist.";
        }
    }
}
