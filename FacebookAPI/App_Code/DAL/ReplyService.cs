using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.DAL
{
    public class ReplyService : ServiceHelper, IReplyService
    {
        #region Constructors
        public ReplyService(IConfiguration configuration, FacebookDbContext context) : base(configuration, context)
        {
            _tableName = _configuration["tableNames:Reply"];
        }
        #endregion

        #region Publci Properties
        public string ReplyAddedOKMessage => $"{_tableName} {_addedOKMessage}";
        #endregion

        #region Public Methods
        public async Task<CoreReply> AddReplyAsync(CoreReply reply)
        {
            var dataReply = new Reply(reply);

            await _context.Replies.AddAsync(dataReply);
            await _context.SaveChangesAsync();

            if (dataReply.ReplyId == 0)
            {
                throw new ApplicationException($"{_tableName} {_couldNotAddedMessage}");
            }

            reply.ReplyId = dataReply.ReplyId;
            reply.DatePosted = dataReply.DatePosted;
            reply.DateUpdated = dataReply.DateUpdated;

            return reply;
        }
        #endregion
    }
}
