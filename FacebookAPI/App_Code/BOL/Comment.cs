using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BOL
{
    public class Comment
    {
        private readonly CoreComment _comment;        

        [Key]
        public int CommentId { get; set; }

        [Required]
        [MaxLength(255)]
        public string CommentBody { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DatePosted { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateUpdated { get; set; }

        public int? PostId { get; set; }
#nullable enable
        public Post? Post { get; set; }

        public int? PictureId { get; set; }
        public Picture? Picture { get; set; }

#nullable disable
        [NotMapped]
        public int LikeCount { get; set; }

        [NotMapped]
        public bool Liked { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<Reply> Replies { get; set; }
        public List<Like> Likes { get; set; }

        public Comment()
        {

        }

        public Comment(CoreComment comment)
        {
            if (comment is null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            _comment = comment;

            if (_comment.CommentId > 0)
            {
                CommentId = _comment.CommentId;
            }

            CommentBody = _comment.CommentBody;
            DatePosted = _comment.DatePosted;
            DateUpdated = _comment.DateUpdated;
            PostId = _comment.PostId;
            PictureId = _comment.PictureId;
            UserId = _comment.UserId;
        }
    }
}
