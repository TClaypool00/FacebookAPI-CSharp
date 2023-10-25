using FacebookAPI.App_Code.BLL;
using System.Text.RegularExpressions;

namespace FacebookAPI.App_Code.DAL
{
    public class PasswordService : IPasswordService
    {
        public string PasswordDoesMeetRequirementsMessage => "Password does not meet our requirements";

        public string IncorrectPasswordMessage => "Incorrect password";

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool PasswordMeetsRequirements(string password)
        {
            return Regex.IsMatch(password, @"^(.{8,20}|[^0-9]*|[^A-Z])$");
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
