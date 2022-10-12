using FacebookAPI_CSharp.DataAccess.PartialModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI_CSharp.DataAccess.Models
{
    public class Post : PartialPostModel
    {
        [Required]
        [Key]
        public int PostId { get; set; }
    }
}
