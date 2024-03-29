﻿using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.ViewModels.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerHelper
    {
        #region Private Fields
        private readonly IFriendService _friendService;
        private readonly IUserService _userService;
        #endregion

        #region Constructors
        public FriendsController(IConfiguration configuration, IFriendService friendService, IUserService userService) : base(configuration)
        {
            _friendService = friendService;
            _userService = userService;
        }
        #endregion

        #region Public Methods
        [HttpPost]
        public async Task<ActionResult> AddFriendRequestAsync([FromBody] PostFriendViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!await _userService.UserExistsAsync(model.ReceiverId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if (!await _userService.UserExistsAsync(model.SenderId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if (!IsUserIdSame(model.SenderId) && !IsAdmin)
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (await _friendService.FriendExistsAsync(model.SenderId, model.ReceiverId))
                {
                    return BadRequest(_friendService.FriendRequestExistsMessage);
                }

                await _friendService.CreateFriendAsync(model.SenderId, model.ReceiverId);

                return Ok(_friendService.FriendAddedMessage);
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpPut]
        public async Task<ActionResult> AcceptFriendAsync([FromBody] PostFriendViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!await _userService.UserExistsAsync(model.ReceiverId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if (!await _userService.UserExistsAsync(model.SenderId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if (!IsUserIdSame(model.ReceiverId) && !IsAdmin)
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (!await _friendService.FriendExistsAsync(model.SenderId, model.ReceiverId))
                {
                    return NotFound(_friendService.FriendRequestDoesNotExistsMessage);
                }

                await _friendService.AcceptFriendAsync(model.SenderId, model.ReceiverId);

                return Ok(_friendService.FriendRequestAcceptedMessage);
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteFriendAsync([FromBody] PostFriendViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (!await _userService.UserExistsAsync(model.ReceiverId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if (!await _userService.UserExistsAsync(model.SenderId))
                {
                    return NotFound(_userService.UserDoesNotExistsMessage);
                }

                if ((!IsUserIdSame(model.ReceiverId) || !IsUserIdSame(model.SenderId)) && !IsAdmin)
                {
                    return Unauthorized(UnAuthorizedMessage);
                }

                if (!await _friendService.FriendExistsAsync(model.SenderId, model.ReceiverId))
                {
                    return NotFound(_friendService.FriendRequestDoesNotExistsMessage);
                }

                await _friendService.DeleteFriendAsync(model.SenderId, model.ReceiverId);

                return Ok(_friendService.FriendDeletedMessage);
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }
        #endregion
    }
}
