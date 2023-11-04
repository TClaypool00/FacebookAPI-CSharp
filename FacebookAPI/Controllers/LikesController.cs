using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.ViewModels;
using FacebookAPI.App_Code.ViewModels.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerHelper
    {
        #region Private fields
        private readonly ILikeService _likeService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IReplyService _replyService;
        private readonly IUserService _userService;
        #endregion

        #region Constructors
        public LikesController(IConfiguration configuration, IPostService postService, ICommentService commentService, IReplyService replyService, ILikeService likeService, IUserService userService) : base(configuration)
        {
            _postService = postService;
            _commentService = commentService;
            _replyService = replyService;
            _likeService = likeService;
            _userService = userService;
        }
        #endregion

        #region Public Methods
        [HttpPost("AddPostLike")]
        public async Task<ActionResult> PostLikePostAsync([FromBody] PostLikeViewModel model)
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

                if (!await _postService.PostExistsAsync(model.Id))
                {
                    return NotFound(_postService.PostDoesNotExistMessage);
                }

                if (await _likeService.LikeExistsAsync(model.Id, model.UserId, Like.LikeOptions.Post))
                {
                    return BadRequest(_likeService.LikeExistMessage);
                }

                var like = await _likeService.AddLikeAsync(model.Id, model.UserId, Like.LikeOptions.Post);

                return Ok(new LikeViewModel(like));
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpPost("AddCommentLike")]
        public async Task<ActionResult> PostLikeCommentAsync([FromBody] PostLikeViewModel model)
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

                if (!await _commentService.CommentExistsAsync(model.Id))
                {
                    return NotFound(_commentService.CommentNotFoundMessage);
                }

                if (await _likeService.LikeExistsAsync(model.Id, model.UserId, Like.LikeOptions.Comment))
                {
                    return BadRequest(_likeService.LikeExistMessage);
                }

                var like = await _likeService.AddLikeAsync(model.Id, model.UserId, Like.LikeOptions.Comment);

                return Ok(new LikeViewModel(like));
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpPost("AddReplyLike")]
        public async Task<ActionResult> PostLikeReplyAsync([FromBody] PostLikeViewModel model)
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

                if (!await _replyService.ReplyExistsAsync(model.Id))
                {
                    return NotFound(_replyService.ReplyNotFoundMessage);
                }

                if (await _likeService.LikeExistsAsync(model.Id, model.UserId, Like.LikeOptions.Reply))
                {
                    return BadRequest(_likeService.LikeExistMessage);
                }

                var like = await _likeService.AddLikeAsync(model.Id, model.UserId, Like.LikeOptions.Reply);

                return Ok(new LikeViewModel(like));
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }
        #endregion
    }
}
