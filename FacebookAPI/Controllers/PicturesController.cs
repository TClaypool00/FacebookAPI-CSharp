using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels;
using FacebookAPI.App_Code.ViewModels.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerHelper
    {
        #region Private Fields
        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        #endregion

        public PicturesController(IConfiguration configuration, IUserService userService, IPictureService pictureService) : base(configuration)
        {
            _userService = userService;
            _pictureService = pictureService;
        }

        [HttpPost]
        public async Task<ActionResult> AddPictureAsync([FromForm] SinglePostPictureViewModel model, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!await _userService.UserExistsAsync(model.UserId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                var corePicture = new CorePicture(model, _configuration, file);
                corePicture = await _pictureService.AddPictureAsync(corePicture);

                if (IsUserIdSame(corePicture.UserId))
                {
                    corePicture.User = new CoreUser(UserId, FirstName, LastName);
                }
                else
                {
                    corePicture.User = await _userService.GetFullNameAsync(corePicture.UserId);
                }

                return Ok(new PictrueViewModel(corePicture));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }
    }
}
