using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacebookAPI.App_Code.BOL
{
    //TODO Add picture service
    public class Picture
    {
        private readonly CorePicture _corePicture;

        public Picture()
        {

        }

        public Picture(CorePicture corePicture)
        {
            if (corePicture is null) 
            { 
                throw new ArgumentNullException(nameof(corePicture)); 
            }

            _corePicture = corePicture;

            if (_corePicture.PictureId > 0)
            {
                PictureId = _corePicture.PictureId;
            }

            CaptionText = _corePicture.CaptionText;
            PictureFileName = _corePicture.NewFileName;
            ProfilePicture = true;
            UserId = _corePicture.UserId;
        }       

        [Key]
        public int PictureId { get; set; }

        [Required]
        [MaxLength(255)]
        public string CaptionText { get; set; }

        [Required]
        [MaxLength(500)]
        public string PictureFileName { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public bool ProfilePicture { get; set; }

        [NotMapped]
        public int LikeCount { get; set; }

        [NotMapped]
        public bool Liked { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
    }
}
