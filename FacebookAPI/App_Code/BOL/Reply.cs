using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    public class Reply
    {
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
    }
}
