using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    public class Like
    {
        private readonly LikeOptions _likeOption;
        private readonly int _id;
        private readonly int _userId;

        public Like()
        {

        }

        public Like(int id, int userId, LikeOptions likeOption)
        {
            _id = id;
            _userId = userId;
            _likeOption = likeOption;

            UserId = _userId;

            switch (_likeOption)
            {
                case LikeOptions.Post:
                    PostId = _id;
                    break;
                case LikeOptions.Comment:
                    CommentId = _id;
                    break;
                case LikeOptions.Reply:
                    ReplyId = _id;
                    break;
                case LikeOptions.Picture:
                    PictureId = _id;
                    break;
                default:
                    throw new ApplicationException("Not a valid option");
            }
        }

        [Key]
        public long LikeId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

#nullable enable
        public int? PostId { get; set; }
        public Post? Post { get; set; }

        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }

        public int? ReplyId { get; set; }
        public Reply? Reply { get; set; }

        public int? PictureId { get; set; }
        public Picture? Picture { get; set; }

        public enum LikeOptions
        {
            Post,
            Comment,
            Reply,
            Picture
        }
    }
}
