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
    public class CommentsController : ControllerHelper
    {
        #region Private fields
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        #endregion

        #region Constructors
        public CommentsController(IConfiguration configuration, ICommentService commentService, IUserService userService, IPostService postService) : base(configuration)
        {
            _commentService = commentService;
            _userService = userService;
            _postService = postService;
        }
        #endregion

        #region Route Methods
        [HttpPost]
        public async Task<ActionResult> AddPostAsync([FromBody] PostCommentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!IsAdmin && !IsUserIdSame(model.UserId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (IsAdmin &&  !IsUserIdSame(model.UserId) && !await _userService.UserExistsAsync(model.UserId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if (!await _postService.PostExistsAsync(model.PostId))
                {
                    return NotFound(_postService.PostDoesNotExistMessage);
                }

                var coreComment = new CoreComment(model);

                coreComment = await _commentService.AddCommentAsync(coreComment);

                if (IsUserIdSame(model.UserId))
                {
                    coreComment.User = new CoreUser(UserId, FirstName, LastName);
                }
                else
                {
                    coreComment.User = await _userService.GetFullNameAsync(model.UserId);
                }

                return Ok(new CommentViewModel(coreComment, _commentService.CommentAddedOKMessag));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCommentAsync(int id, [FromBody] PostCommentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!await _commentService.CommentExistsAsync(id))
                {
                    return NotFound(_commentService.CommentNotFoundMessage);
                }

                if (!IsAdmin && !IsUserIdSame(model.UserId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (IsAdmin && !IsUserIdSame(model.UserId) && !await _userService.UserExistsAsync(model.UserId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if (!await _postService.PostExistsAsync(model.PostId))
                {
                    return NotFound(_postService.PostDoesNotExistMessage);
                }

                var coreComment = new CoreComment(model, id);

                coreComment = await _commentService.UpdateCommentAsync(coreComment);

                if (IsUserIdSame(model.UserId))
                {
                    coreComment.User = new CoreUser(UserId, FirstName, LastName);
                }
                else
                {
                    coreComment.User = await _userService.GetFullNameAsync(model.UserId);
                }

                return Ok(new CommentViewModel(coreComment, _commentService.CommentUpdatedOKMessage));

            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }
        #endregion
    }
}
