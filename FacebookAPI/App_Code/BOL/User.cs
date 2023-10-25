using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacebookAPI.App_Code.BOL
{
    public class User
    {
        private readonly CoreUser _user;

        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }


        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }

        public bool IsAdmin { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

#nullable enable
        [NotMapped]
        public Picture? ProfilePicture { get; set; }

#nullable disable
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Reply> Replies { get; set; }
        public List<Location> Locations { get; set; }
        public List<Interest> Interests { get; set; }
        public List<GroupUser> GroupUsers { get; set; }
        public List<IssueTask> IssueTasks { get; set; }
        public List<Picture> Pictures { get; set; }


        public User()
        {

        }

        public User(CoreUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _user = user;

            if (string.IsNullOrEmpty(_user.Password))
            {
                throw new ApplicationException("Password is empty");
            }

            if (_user.UserId != 0)
            {
                UserId = _user.UserId;
            }

            FirstName = _user.FirstName;
            LastName = _user.LastName;
            Email = _user.Email;
            Password = _user.Password;
            PhoneNumber = _user.PhoneNumber;
            IsAdmin = _user.IsAdmin;
            ProfileId = _user.ProfileId;
            Password = _user.Password;
        }
    }
}
