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

        [HttpGet]
        public async Task<ActionResult> GetPicturesAsync([FromQuery] int? index = null, int? postId = null)
        {
            try
            {
                var pictures = await _pictureService.GetPictureAsync(UserId, index, postId);

                if (pictures.Count == 0)
                {
                    return NotFound(_pictureService.PicturesNotFoundMessage);
                }

                var pictureViewModels = new List<PictrueViewModel>();

                for (int i = 0; i < pictures.Count; i++)
                {
                    pictureViewModels.Add(new PictrueViewModel(pictures[i]));
                }

                return Ok(pictureViewModels);
            }
            catch (Exception e) 
            {
                return InternalError(e);
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

        [HttpPut("UpdateProfilePicture/{id}")]
        public async Task<ActionResult> UpdateProfilePictureAsync(int id, [FromBody] int userId, bool profilePicture)
        {
            try
            {
                if (!IsAdmin && IsUserIdSame(userId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (IsAdmin && !await _pictureService.PictureExistsAsync(id))
                {
                    return NotFound(_pictureService.PictureDoesNotExistMessage);
                }

                if (!IsAdmin && !await _pictureService.UserOwnsPictureAsync(id, UserId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                await _pictureService.UpdateProfilePictureAsync(id, profilePicture, userId);

                return Ok(_pictureService.PictureUpdatedOKMessage);
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
                    await _pictureService.UpdateProfilePictureAsync(id, model.ProfilePicture, model.UserId);
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
