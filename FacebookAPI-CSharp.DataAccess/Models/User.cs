using System.ComponentModel.DataAnnotations;

namespace FacebookAPI_CSharp.DataAccess.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [Required]
        public bool IsAdmin { get; set; } = false;

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(10)]
        public string PhoneNum { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        public List<Post> Posts { get; set; }
    }
}
