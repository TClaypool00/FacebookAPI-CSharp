using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
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

                if (!await _userService.UserExistsAsync(model.UserId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if (!IsUserIdSame(UserId) && !IsAdmin)
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                var corePost = new CorePost(model);
                corePost = await _postService.AddPostAsync(corePost);

                if (IsUserIdSame(model.UserId))
                {
                    corePost.User = new CoreUser(UserId, FirstName, LastName);
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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePostAsync(int id, [FromBody] PostPostViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (IsAdmin && !await _postService.PostExistsAsync(id))
                {
                    return NotFound(_postService.PostDoesNotExistMessage);
                }

                if (!IsAdmin)
                {
                    if (!IsUserIdSame(model.UserId))
                    {
                        return Unauthorized(UnAuthorizedMessage);
                    }

                    if (!await _postService.UserHasAccessToPostAsync(id, UserId))
                    {
                        return Unauthorized(_postService.UserDoesNotHaveAccessMessage);
                    }
                }

                if (!await _userService.UserExistsAsync(model.UserId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                var corePost = new CorePost(model, id);
                corePost = await _postService.UpdatePostAsync(corePost);

                if (IsUserIdSame(model.UserId))
                {
                    corePost.User = new CoreUser(UserId, FirstName, LastName);
                }
                else
                {
                    corePost.User = await _userService.GetFullNameAsync(model.UserId);
                }

                return Ok(new PostViewModel(corePost));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPostAsync(int id, [FromQuery] bool? includeComments = null)
        {
            try
            {
                if (!await _postService.UserHasAccessToPostAsync(id, UserId) && !IsAdmin)
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (IsAdmin && !await _postService.PostExistsAsync(id))
                {
                    return NotFound(_postService.PostDoesNotExistMessage);
                }

                var corePost = await _postService.GetPostByIdAsync(id, UserId, includeComments);

                return Ok(new PostViewModel(corePost));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPosts([FromQuery] int userId, int? index = null, bool? includeComments = null)
        {
            try
            {
                if (!IsAdmin && !IsUserIdSame(userId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                var corePosts = await _postService.GetAllPostsAsync(userId, index, includeComments);
                
                if (corePosts.Count == 0)
                {
                    return NotFound(_postService.NoPostsFoundMessage);
                }

                var postViewModels = new List<PostViewModel>();

                for (int i = 0; i < corePosts.Count; i++)
                {
                    postViewModels.Add(new PostViewModel(corePosts[i]));
                }

                return Ok(postViewModels);

            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpGet("GetFriendsPosts")]
        public async Task<ActionResult> GetFriendsPosts(int userId, int? index = null)
        {
            try
            {
                if (!IsAdmin && !IsUserIdSame(userId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                var corePosts = await _postService.GetFriendsPostsAsync(userId, index);

                if (corePosts.Count == 0)
                {
                    return NotFound(_postService.NoPostsFoundMessage);
                }

                var postViewModels = new List<PostViewModel>();

                for (int i = 0; i < corePosts.Count; i++)
                {
                    postViewModels.Add(new PostViewModel(corePosts[i]));
                }

                return Ok(postViewModels);

            }
            catch (Exception exception)
            {
                return InternalError(exception);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePostAsync(int id)
        {
            try
            {
                if (!IsAdmin && !await _postService.UserHasAccessToPostAsync(id, UserId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (IsAdmin && !await _postService.PostExistsAsync(id))
                {
                    return NotFound(_postService.PostDoesNotExistMessage);
                }

                await _postService.DeletePostAsync(id);

                return Ok(_postService.PostDeletedMessage);

            }
            catch (Exception exception)
            {
                return InternalError(exception);
            }
        }
        #endregion
    }
}
