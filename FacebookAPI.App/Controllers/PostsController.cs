using FacebookAPI.App.Models.ApiModels;
using FacebookAPI.App.Models.PostModels;
using FacebookAPI.Core.Interfaces;
using FacebookAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerHelper
    {
        private readonly IPostService _service;
        private readonly IUserService _userService;

        public PostsController(IPostService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<List<ApiPostModel>>> GetPosts(int? userId = null)
        {
            var posts = await _service.GetPostsAsync(userId);

            if (posts.Count > 0)
            {
                return Ok(posts.Select(ApiMapper.MapPost).ToList());
            }

            return NotFound("No posts found");
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiPostModel>> GetPost(int id)
        {
            if (await _service.PostExistsAsync(id))
            {
                var post = await _service.GetPostByIdAsync(id);

                return Ok(ApiMapper.MapPost(post));
            }

            return NotFound(PostNotFound(id));
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, ApiPostTextModel post)
        {
            try
            {
                if (!await _service.PostExistsAsync(id))
                {
                    return NotFound(PostNotFound(id));
                }

                if (!await _userService.UserExistsAsync(post.UserId))
                {
                    return BadRequest(UsersController.UserDoesNotExist(post.UserId));
                }

                var corePost = await ApiMapper.MapPost(post, _userService);

                corePost = await _service.UpdatePostAsync(corePost, id);

                return Ok(ApiMapper.MapPost(corePost));
            } catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(500, UserErrorMessage());
            }
        }

        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(ApiPostTextModel post)
        {
            if (!await _userService.UserExistsAsync(post.UserId))
            {
                return NotFound(UsersController.UserDoesNotExist(post.UserId));
            }

            var corePost = await ApiMapper.MapPost(post, _userService);

            corePost = await _service.AddPostAsync(corePost);

            return Ok(ApiMapper.MapPost(corePost));
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (!await _service.PostExistsAsync(id)) {
                return NotFound(PostNotFound(id));
            }

            if (await _service.DeletePostAsync(id))
            {
                return Ok("Post has been delted");
            }

            return StatusCode(500, UserErrorMessage());
        }

        private string PostNotFound(int id)
        {
            return $"Post with an Id of {id} could not found";
        }
    }
}
