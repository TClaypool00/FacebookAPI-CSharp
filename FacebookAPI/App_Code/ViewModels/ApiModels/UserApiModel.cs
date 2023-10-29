using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.ViewModels.ApiModels
{
    public class UserApiModel : UserViewModel
    {
        public UserApiModel()
        {
            
        }

        public UserApiModel(CoreUser coreUser, string token) : base(coreUser) 
        {
            Token = token;
        }

        public string Token { get; set; }
    }
}
