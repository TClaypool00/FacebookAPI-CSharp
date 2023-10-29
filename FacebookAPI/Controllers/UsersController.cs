using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels;
using FacebookAPI.App_Code.ViewModels.ApiModels;
using FacebookAPI.App_Code.ViewModels.FullModels;
using FacebookAPI.App_Code.ViewModels.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FacebookAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerHelper
    {
        #region Private fields
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        #endregion

        #region Constructors
        public UsersController(IUserService userService, IPasswordService passwordService, IConfiguration configuration) : base(configuration)
        {
            _userService = userService;
            _passwordService = passwordService;
        }
        #endregion

        #region Public methods
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateUserAsync([FromBody] RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (await _userService.EmailExistsAsync(model.Email))
                {
                    return BadRequest(_userService.EmailExistsMessage);
                }

                if (await _userService.PhoneNumberExistsAsync(model.PhoneNumber))
                {
                    return BadRequest(_userService.PhoneNumberExistsMessage);
                }

                if (!_passwordService.PasswordMeetsRequirements(model.Password))
                {
                    return BadRequest(_passwordService.PasswordDoesMeetRequirementsMessage);
                }

                model.Password = _passwordService.HashPassword(model.Password);

                var coreUser = new CoreUser(model);
                await _userService.CreateUserAsync(coreUser);

                return Ok(_userService.UserCreatedMessage);
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!await _userService.EmailExistsAsync(model.Email))
                {
                    return NotFound(_userService.EmailDoesNotExistMessage);
                }

                var user = await _userService.GetUserAsync(model.Email);

                if (!_passwordService.VerifyPassword(model.Password, user.Password))
                {
                    return BadRequest(_passwordService.IncorrectPasswordMessage);
                }

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("PhoneNumber", user.PhoneNumber),
                    new Claim("Email", user.Email),
                    new Claim("IsAdmin", user.IsAdmin.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretConfig.SecretKey));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["JWT:Issuer"],
                    _configuration["JWT:Audience"],
                    claims,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWT:TimeoutLength"])),
                    signIn
                    );

                var apiUser = new UserApiModel(user, new JwtSecurityTokenHandler().WriteToken(token));

                return Ok(apiUser);
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserAsync(int id)
        {
            try
            {
                if (!await _userService.UserExistsAsync(id))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if (!IsUserIdSame(id) && !IsAdmin)
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                var user = await _userService.GetUserAsync(id);

                return Ok(new UserViewModel(user));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserAsync(int id, [FromBody] PostUserProfileViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!await _userService.UserExistsAsync(id))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if (!IsUserIdSame(id) && !IsAdmin)
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (await _userService.EmailExistsAsync(model.Email, id))
                {
                    return BadRequest(_userService.EmailExistsMessage);
                }

                if (await _userService.PhoneNumberExistsAsync(model.PhoneNumber, id))
                {
                    return BadRequest(_userService.PhoneNumberExistsMessage);
                }

                var coreUser = new CoreUser(id, model);

                coreUser = await _userService.UpdateUserAsync(id, coreUser);

                return Ok(new FullUserProfileViewModel(coreUser, _userService.UpdatePasswordSuccessMessage));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }
        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] PostChangePasswordViewModel model)
        {
            //TODO: Fix logic so it doesn't have to make 2 calls to the database
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!await _userService.UserExistsAsync(UseerId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                var user = await _userService.GetUserAsync(UseerId, true);

                if (!_passwordService.VerifyPassword(model.Password, user.Password))
                {
                    return BadRequest(_passwordService.IncorrectPasswordMessage);
                }

                if (!_passwordService.PasswordMeetsRequirements(model.NewPassword))
                {
                    return BadRequest(_passwordService.PasswordDoesMeetRequirementsMessage);
                }

                model.NewPassword = _passwordService.HashPassword(model.NewPassword);

                await _userService.UpdatePasswordAsync(UseerId, model.NewPassword);

                return Ok(_userService.UpdatePasswordSuccessMessage);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }
        #endregion
    }
}
