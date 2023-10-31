using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BOL
{
    public class Reply
    {
        #region Private Read-Only Fields
        private readonly CoreReply _coreReply;
        #endregion

        #region Constructors
        public Reply()
        {
            
        }

        public Reply(CoreReply coreReply)
        {
            _coreReply = coreReply ?? throw new ArgumentNullException(nameof(coreReply));

            if (_coreReply.ReplyId > 0)
            {
                ReplyId = _coreReply.ReplyId;
            }

            ReplyBody = _coreReply.ReplyBody;
            CommentId = _coreReply.CommentId;
            UserId = _coreReply.UserId;
        }
        #endregion

        #region Public Properties
        [Key]
        public int ReplyId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ReplyBody { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DatePosted { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateUpdated { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [NotMapped]
        public int LikeCount { get; set; }

        [NotMapped]
        public bool Liked { get; set; }

        public List<Like> Likes { get; set; }
        #endregion
    }
}
