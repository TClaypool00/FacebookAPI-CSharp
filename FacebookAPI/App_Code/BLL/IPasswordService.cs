namespace FacebookAPI.App_Code.BLL
{
    public interface IPasswordService
    {
        public string HashPassword(string password);

        public bool VerifyPassword(string password, string hash);

        public bool PasswordMeetsRequirements(string password);

        public string PasswordDoesMeetRequirementsMessage { get; }
        
        public string IncorrectPasswordMessage { get; }
    }
}
