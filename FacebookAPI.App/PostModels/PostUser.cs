using FacebookAPI.App.ApiModels;

namespace FacebookAPI.App.PostModels
{
    public class PostUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public bool PasswordsAreTheSame()
        {
            return Password == ConfirmPassword;
        }
    }
}
