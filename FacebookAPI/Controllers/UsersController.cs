using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.Controllers
{
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
        #endregion
    }
}
