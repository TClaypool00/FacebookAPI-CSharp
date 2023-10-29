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
    public class PostsController : ControllerHelper
    {
        #region Private fields
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        #endregion

        #region Constructors
        public PostsController(IConfiguration configuration, IPostService postService, IUserService userService) : base(configuration)
        {
            _postService = postService;
            _userService = userService;
        }
        #endregion

        #region Route Methods
        [HttpPost]
        public async Task<ActionResult> AddPostAsync([FromBody] PostPostViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!IsUserIdSame(UseerId) && !IsAdmin)
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                var corePost = new CorePost(model);
                corePost = await _postService.AddPostAsync(corePost);

                if (IsUserIdSame(model.UserId))
                {
                    corePost.User = new CoreUser(UseerId, FirstName, LastName);
                }
                else
                {
                    corePost.User = await _userService.GetUserAsync(model.UserId);
                }

                return Ok(new PostViewModel(corePost));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }
        #endregion
    }
}
