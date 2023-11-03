using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RepliesController : ControllerHelper
    {
        #region Private fields
        private readonly IReplyService _replyService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;
        #endregion

        #region Constructors
        public RepliesController(IConfiguration configuration, IReplyService replyService, IUserService userService, ICommentService commentService) : base(configuration)
        {
            _replyService = replyService;
            _userService = userService;
            _commentService = commentService;
        }
        #endregion

        #region Public Route Methods
        [HttpPost]
        public async Task<ActionResult> AddReplyAsync([FromBody] PostReplyViewModel model)
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

                if (!IsAdmin && !IsUserIdSame(model.UserId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (!await _commentService.CommentExistsAsync(model.CommentId))
                {
                    return NotFound(_commentService.CommentNotFoundMessage);
                }

                var coreReply = new CoreReply(model);
                coreReply = await _replyService.AddReplyAsync(coreReply);

                if (IsUserIdSame(model.UserId))
                {
                    coreReply.User = new CoreUser(UserId, FirstName, LastName);
                }
                else
                {
                    coreReply.User = await _userService.GetFullNameAsync(model.UserId);
                }

                return Ok(new ReplyViewModel(coreReply, _replyService.ReplyAddedOKMessage));
                
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReplyAsync(int id, [FromBody] PostReplyViewModel model)
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

                if (!IsAdmin && !IsUserIdSame(model.UserId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (!await _commentService.CommentExistsAsync(model.CommentId))
                {
                    return NotFound(_commentService.CommentNotFoundMessage);
                }

                var coreReply = new CoreReply(model, id);
                coreReply = await _replyService.UpdateReplyAsync(coreReply);

                if (IsUserIdSame(model.UserId))
                {
                    coreReply.User = new CoreUser(UserId, FirstName, LastName);
                }
                else
                {
                    coreReply.User = await _userService.GetFullNameAsync(model.UserId);
                }

                return Ok(new ReplyViewModel(coreReply, _replyService.ReplyUpdatedOKMessage));
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetReplyByIdAsync(int id)
        {
            try
            {
                if (IsAdmin && !await _replyService.ReplyExistsAsync(id))
                {
                    return NotFound(_replyService.ReplyNotFoundMessage);
                }

                if (!IsAdmin && !await _replyService.UserHasAccessToReplyAsync(id, UserId))
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                var reply = await _replyService.GetReplyAsync(id);

                return Ok(new ReplyViewModel(reply));
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRepliesAsync([FromQuery] int? index = null, int? userId = null, int? commentId = null, int? postId = null)
        {
            try
            {
                if (AllParametersNull(userId, commentId, postId))
                {
                    return BadRequest(AllParametersNullMessage);
                }

                if (userId is null && !IsUserIdSame(userId) && !IsAdmin)
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                var coreReplies = await _replyService.GetRepliesAsync(index, userId, commentId, postId);

                if (coreReplies.Count == 0)
                {
                    return NotFound(_replyService.NoRepliesFoundMessage);
                }

                var replyViewModels = new List<ReplyViewModel>();

                for (var i = 0; i < coreReplies.Count; i++)
                {
                    replyViewModels.Add(new ReplyViewModel(coreReplies[i]));
                }

                return Ok(replyViewModels);

            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }
        #endregion
    }
}
