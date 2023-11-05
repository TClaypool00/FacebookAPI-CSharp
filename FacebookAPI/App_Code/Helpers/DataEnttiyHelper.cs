using FacebookAPI.App_Code.BOL;

namespace FacebookAPI.App_Code.Helpers
{
    public class DataEnttiyHelper
    {
        #region Private Fields
        private List<int> _ids;
        #endregion

        #region Public Methods
        public List<int> GetIds(List<Comment> comments)
        {
            _ids = new List<int>();

            for (int i = 0; i < comments.Count; i++)
            {
                if (!Exists(comments[i].CommentId))
                {
                    _ids.Add(comments[i].CommentId);
                }
            }

            return _ids;
        }

        public List<int> GetIds(List<Reply> replies)
        {
            _ids = new List<int>();

            for (int i = 0; i < replies.Count; i++)
            {
                if (!Exists(replies[i].ReplyId))
                {
                    _ids.Add(replies[i].ReplyId);
                }
            }

            return _ids;
        }
        #endregion

        #region Private methods
        private bool Exists(int id)
        {
            return _ids.Any(x => x == id);
        }
        #endregion
    }
}
