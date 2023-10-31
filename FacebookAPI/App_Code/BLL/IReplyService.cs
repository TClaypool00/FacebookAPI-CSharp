using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IReplyService
    {
        #region Public Methods
        public Task<CoreReply> AddReplyAsync(CoreReply reply);
        #endregion

        #region Public Properties
        public string ReplyAddedOKMessage { get; }
        #endregion
    }
}
