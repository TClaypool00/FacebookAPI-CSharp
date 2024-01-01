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
        private readonly ICommentService _commentService;
        #endregion

        public PicturesController(IConfiguration configuration, IUserService userService, IPictureService pictureService, ICommentService commentService) : base(configuration)
        {
            _userService = userService;
            _pictureService = pictureService;
            _commentService = commentService;
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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPictureAsync(int id, [FromQuery] bool? includeComments = null, bool? includeReplies = null)
        {
            try
            {
                if (!await _pictureService.PictureExistsAsync(id))
                {
                    return NotFound(_pictureService.PictureDoesNotExistMessage);
                }

                var picture = await _pictureService.GetPictureByIdAsync(id, UserId);

                if (includeComments == true)
                {
                    picture.Comments = await _commentService.GetCommentsAsync(UserId, null, null, null, includeReplies, id);
                }

                return Ok(new PictrueViewModel(picture));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePictureAsync(int id, [FromBody] SinglePostPictureViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!await _pictureService.PictureExistsAsync(id))
                {
                    return NotFound(_pictureService.PictureDoesNotExistMessage);
                }

                var corePicture = await _pictureService.GetPictureByIdAsync(id, UserId);

                if (!IsAdmin)
                {
                    if (corePicture.UserId !=  model.UserId || corePicture.PostId != model.PostId)
                    {
                        return Unauthorized(UnAuthorizedMessage);
                    }
                }

                if (corePicture.ProfilePicture == false && model.ProfilePicture == true)
                {
                    await _pictureService.UpdateProfilePictureAsync(id, model.ProfilePicture);
                }

                var updatedCorePicture = new CorePicture(model, id);

                updatedCorePicture = await _pictureService.UpdatePictureByIdAsync(updatedCorePicture);

                if (IsUserIdSame(model.UserId))
                {
                    updatedCorePicture.User = new CoreUser(UserId, FirstName, LastName);
                }
                else
                {
                    updatedCorePicture.User = await _userService.GetFullNameAsync(model.UserId);
                }

                return Ok(new PictrueViewModel(updatedCorePicture));

            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePicture(int id)
        {
            try
            {
                if (IsAdmin && !await _pictureService.PictureExistsAsync(id))
                {
                    return NotFound(_pictureService.PictureDoesNotExistMessage);
                }

                if (!IsAdmin && !await _pictureService.UserOwnsPictureAsync(id, UserId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                await _pictureService.DeletePictureAsync(id);

                return Ok(_pictureService.PictureDeletedOKMessage);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }
    }
}
