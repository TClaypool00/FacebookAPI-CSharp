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
        private readonly IPictureService _pictureService;
        #endregion

        #region Constructors
        public CommentsController(IConfiguration configuration, ICommentService commentService, IUserService userService, IPostService postService, IPictureService pictureService) : base(configuration)
        {
            _commentService = commentService;
            _userService = userService;
            _postService = postService;
            _pictureService = pictureService;
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

                if (AllParametersNull(model.PostId, model.PictureId))
                {
                    return BadRequest(_configuration["Comment:PostCommentError"]);
                }

                if (!IsAdmin && !IsUserIdSame(model.UserId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (IsAdmin &&  !IsUserIdSame(model.UserId) && !await _userService.UserExistsAsync(model.UserId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if (model.PostId is not null)
                {
                    if (!await _postService.PostExistsAsync((int)model.PostId))
                    {
                        return NotFound(_postService.PostDoesNotExistMessage);
                    }
                }

                if (model.PictureId is not null)
                {
                    if (! await _pictureService.PictureExistsAsync((int)model.PictureId))
                    {
                        return NotFound(_pictureService.PictureDoesNotExistMessage);
                    }
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

                if (AllParametersNull(model.PostId, model.PictureId))
                {
                    return BadRequest(_configuration["Comment:PostCommentError"]);
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

                if (model.PostId is not null)
                {
                    if (!await _postService.PostExistsAsync((int)model.PostId))
                    {
                        return NotFound(_postService.PostDoesNotExistMessage);
                    }
                }

                if (model.PictureId is not null)
                {
                    if (!await _pictureService.PictureExistsAsync((int)model.PictureId))
                    {
                        return NotFound(_pictureService.PictureDoesNotExistMessage);
                    }
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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCommentByIdAync(int id, [FromQuery] bool? includeComments = null)
        {
            try
            {
                if (!IsAdmin && !await _commentService.UserHasAccessToCommentAsync(id, UserId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (IsAdmin && !await _commentService.CommentExistsAsync(id))
                {
                    return NotFound(_commentService.CommentNotFoundMessage);
                }

                var coreComment = await _commentService.GetCommentAsync(id, UserId, includeComments);

                return Ok(new CommentViewModel(coreComment));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCommentsAsync([FromQuery] int? userId = null, int? index = null, int? postId = null, bool? includeReplies = null, int? pictureId = null)
        {
            try
            {
                if (AllParametersNull(userId, postId, pictureId))
                {
                    return BadRequest(AllParametersNullMessage);
                }

                if (!IsAdmin && !IsUserIdSame(userId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                var coreComments = await _commentService.GetCommentsAsync(UserId, userId, index, postId, includeReplies, pictureId);

                if (coreComments.Count == 0)
                {
                    return NotFound(_commentService.NoCommentsFound);
                }

                var commentViewModels = new List<CommentViewModel>();

                for (int i = 0; i < coreComments.Count; i++)
                {
                    commentViewModels.Add(new CommentViewModel(coreComments[i]));
                }

                return Ok(commentViewModels);
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCommentAsync(int id)
        {
            try
            {
                if (!IsAdmin && !await _commentService.UserHasAccessToCommentAsync(id, UserId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (IsAdmin && !await _commentService.CommentExistsAsync(id))
                {
                    return NotFound(_commentService.NoCommentsFound);
                }

                await _commentService.DeleteCommentAsync(id);

                return Ok(_commentService.CommentDeletedOKMessage);
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }
        #endregion
    }
}
