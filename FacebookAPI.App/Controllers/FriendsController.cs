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
    public class FriendsController : ControllerHelper
    {
        private readonly IFriendService _service;
        private readonly IUserService _userService;

        public FriendsController(IFriendService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        // POST: api/Friends
        [HttpPost]
        public async Task<ActionResult> PostFriend([FromBody]ApiPostFriend friend)
        {
            try
            {
                if (!await _service.FriendExists(friend.SenderId, friend.ReceiverId))
                {
                    if (friend.SenderReciverIdSame())
                    {
                        return BadRequest(friend.IdsCannotBeSame);
                    }

                    if (!await _userService.UserExistsAsync(friend.SenderId))
                    {
                        return NotFound(UsersController.UserDoesNotExist(friend.SenderId));
                    }

                    if (!await _userService.UserExistsAsync(friend.ReceiverId))
                    {
                        return NotFound(UsersController.UserDoesNotExist(friend.ReceiverId));
                    }

                    var coreFriend = await ApiMapper.MapFriend(friend, _userService);
                    await _service.AddFriendAsync(coreFriend);

                    return Ok("Friend request has been sent");
                } else
                {
                    return BadRequest(NoFriendExists(friend.SenderId, friend.SenderId, true));
                }
            } catch (Exception)
            {
                return StatusCode(500, UserErrorMessage());
            }
        }

        [HttpPut("accept")]
        public async Task<ActionResult> AcceptFriendAsync([FromBody] ApiPostFriend friend)
        {
            try
            {
                if (await _service.FriendExists(friend.SenderId, friend.ReceiverId))
                {
                    if (friend.SenderReciverIdSame())
                    {
                        return BadRequest(friend.IdsCannotBeSame);
                    }

                    if (!await _service.FriendExists(friend.SenderId, friend.ReceiverId))
                    {
                        return NotFound(UsersController.UserDoesNotExist(friend.SenderId));
                    }

                    if (!await _userService.UserExistsAsync(friend.SenderId))
                    {
                        return NotFound(UsersController.UserDoesNotExist(friend.SenderId));
                    }

                    if (!await _userService.UserExistsAsync(friend.ReceiverId))
                    {
                        return NotFound(UsersController.UserDoesNotExist(friend.ReceiverId));
                    }

                    var coreFriend = await _service.GetFrinedAsync(friend.SenderId, friend.ReceiverId);

                    await _service.AddFriendAsync(coreFriend);

                    return Ok(true);
                } else
                {
                    return NotFound(NoFriendExists(friend.SenderId, friend.ReceiverId));
                }
            } catch (Exception)
            {
                return StatusCode(500, UserErrorMessage());
            }
        }

        // DELETE: api/Friends/5
        [HttpDelete("{senderId}&{receiverId}")]
        public async Task<IActionResult> DeleteFriend(int senderId, int receiverId)
        {
            try
            {
                if (await _service.FriendExists(senderId, receiverId))
                {
                    await _service.DeleteFriendAsync(senderId, receiverId);
                    return Ok("Friend has been deleted");
                } else
                {
                    return NotFound(NoFriendExists(senderId, senderId));
                }
            } catch (Exception)
            {
                return StatusCode(500, UserErrorMessage());
            }
        }

        private static string NoFriendExists(int senderId, int receiverId, bool exists = false)
        {
            return $"A friend with a sender id of {senderId} and {receiverId}" + (exists ? " already exists" : "does not exist");
        }
    }
}
