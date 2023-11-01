﻿using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IReplyService
    {
        #region Public Methods
        public Task<CoreReply> AddReplyAsync(CoreReply reply);

        public Task<CoreReply> UpdateReplyAsync(CoreReply reply);

        public Task<bool> UserHasAccessToReplyAsync(int id, int userId);

        public Task<bool> ReplyExistsAsync(int id);
        #endregion

        #region Public Properties
        public string ReplyAddedOKMessage { get; }

        public string ReplyUpdatedOKMessage { get; }

        public string ReplyNotFoundMessage { get; }
        #endregion
    }
}