using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.ViewModels;
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
        #endregion

        #region Constructors
        public LikesController(IConfiguration configuration, IPostService postService, ICommentService commentService, IReplyService replyService, ILikeService likeService) : base(configuration)
        {
            _postService = postService;
            _commentService = commentService;
            _replyService = replyService;
            _likeService = likeService;
        }

        #region Public Methods
        [HttpPost("AddCommentLike")]
        public async Task<ActionResult> PostLikeCommentAsync([FromBody] IdViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!await _commentService.CommentExistsAsync(model.Id))
                {
                    return NotFound(_commentService.CommentNotFoundMessage);
                }

                if (await _likeService.LikeExistsAsync(model.Id, UserId, Like.LikeOptions.Comment))
                {
                    return BadRequest(_likeService.LikeExistMessage);
                }

                var like = await _likeService.AddLikeAsync(model.Id, UserId, Like.LikeOptions.Comment);

                return Ok(new LikeViewModel(like));
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }
        #endregion
        #endregion
    }
}
