using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface ICommentService
    {
        public Task<CoreComment> AddCommentAsync(CoreComment comment);
    }
}
