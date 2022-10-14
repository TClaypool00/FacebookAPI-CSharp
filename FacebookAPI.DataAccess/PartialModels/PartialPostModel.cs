using FacebookAPI.DataAccess.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacebookAPI.DataAccess.PartialModels
{
    public class PartialPostModel
    {
        [Required]
        [MaxLength(255)]
        public string Body { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DatePosted { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
