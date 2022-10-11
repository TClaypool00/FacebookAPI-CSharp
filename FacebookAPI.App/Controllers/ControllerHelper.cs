using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.App.Controllers
{
    public class ControllerHelper : ControllerBase
    {
        public string UserErrorMessage()
        {
            return "An error has occurred. We apologize and will fix the error as soon as possible.";
        }
    }
}
