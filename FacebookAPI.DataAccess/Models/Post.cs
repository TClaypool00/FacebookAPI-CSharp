using FacebookAPI.DataAccess.PartialModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.DataAccess.Models;

public class Post : PartialPostModel
{
    [Required]
    [Key]
    public int PostId { get; set; }
}
