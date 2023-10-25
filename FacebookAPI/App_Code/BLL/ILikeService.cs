using static FacebookAPI.App_Code.BOL.Like;

namespace FacebookAPI.App_Code.BLL
{
    public interface ILikeService
    {
        public Task<Tuple<int, bool>> AddLikeAsync(int id, int userId, LikeOptions likeOptions);

        public Task<Tuple<int, bool>> DeleteLikeAsync(int id, int userId, LikeOptions likeOptions);

        public Task<bool> LikeExistsAsync(int id, int userId, LikeOptions likeOptions);

        public string LikeDoesNotExistMessage { get; }

        public string LikeExistMessage { get; }

        public string LikeIdMessage { get; }
    }
}
