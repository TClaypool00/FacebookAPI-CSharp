using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels.DropdownModels;
using FacebookAPI.App_Code.ViewModels.FullModels;
using FacebookAPI.App_Code.ViewModels.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerHelper
    {
        #region Private Fields
        private readonly IGenderService _genderService;
        private readonly IProfileService _profileService;
        private readonly IUserService _userService;
        #endregion

        #region Constructors
        public ProfilesController(IConfiguration configuration, IGenderService genderService, IProfileService profileService, IUserService userService) : base(configuration)
        {
            _genderService = genderService;
            _profileService = profileService;
            _userService = userService;
        }
        #endregion

        #region Public Methods
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProfileAsync(int id, [FromBody] PostUserProfileViewModel model)
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

                coreUser = await _profileService.UpdateUserAsync(id, coreUser);

                return Ok(new FullUserProfileViewModel(coreUser, _profileService.UpdateProfileOKMessage));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserProfileAsync(int id, [FromQuery] bool? includeDropDown = null)
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

                var coreUser = await _profileService.GetUserProfileAsync(id, UserId);

                if (includeDropDown == true)
                {
                    var genderDropDown = await _genderService.GetGenderDropDownAsync(coreUser.Profile.GenderId);

                    return Ok(new UserProfileWithDropDown(coreUser, genderDropDown));
                }

                return Ok(new FullUserProfileViewModel(coreUser));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }
        #endregion
    }
}
