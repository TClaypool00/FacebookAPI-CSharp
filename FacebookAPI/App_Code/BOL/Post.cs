using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacebookAPI.App_Code.BOL
{
    public class Post
    {
        private readonly CorePost _post;

        [Key]
        public int PostId { get; set; }

        [Required]
        [MaxLength(255)]
        public string PostBody { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DatePosted { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateUpdated { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [NotMapped]
        public int LikeCount { get; set; }

        [NotMapped]
        public bool Liked { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
        public List<Picture> Pictures { get; set; }


        public Post()
        {

        }

        public Post(CorePost post)
        {
            if (post is null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            _post = post;

            if (_post.PostId > 0)
            {
                PostId = _post.PostId;
            }

            PostBody = _post.PostBody;
            UserId = _post.UserId;
            DatePosted = _post.DatePosted;
            DateUpdated = _post.DateUpdated;

            if (_post.Pictures is not null && _post.Pictures.Count > 0)
            {
                Pictures = new List<Picture>();
            }
        }
    }
}
