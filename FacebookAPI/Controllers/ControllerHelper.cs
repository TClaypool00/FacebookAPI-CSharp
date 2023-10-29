using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.Controllers
{
    public class ControllerHelper : ControllerBase
    {
        #region Private fields
        private readonly string _internalMessage;
        private readonly string _unauthorizedMessage;
        #endregion

        #region Protected fields
        protected readonly IConfiguration _configuration;
        #endregion

        #region Constructors
        public ControllerHelper(IConfiguration configuration) : base()
        {
            _configuration = configuration;
            _internalMessage = _configuration.GetSection("Messages").GetSection("Internal").Value;
            _unauthorizedMessage = _configuration["Messages:Unauthorized"];
        }
        #endregion

        #region Protected Properties
        #region User Claims
        protected int UseerId
        {
            get
            {
                return int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            }
        }

        protected string FirstName
        {
            get
            {
                return User.Claims.FirstOrDefault(c => c.Type == "FirstName").Value;
            }
        }

        protected string LastName
        {
            get
            {
                return User.Claims.FirstOrDefault(c => c.Type == "LastName").Value;
            }
        }

        protected bool IsAdmin
        {
            get
            {
                return User.Claims.FirstOrDefault(c => c.Type == "IsAdmin").Value == "True";
            }
        }
        #endregion

        #region Messages
        protected string InternalErrorMessage
        {
            get
            {
                return _internalMessage;
            }
        }

        protected string UnAuthorizedMessage
        {
            get
            {
                return _unauthorizedMessage;
            }
        }
        #endregion
        #endregion

        #region Protected methods
        protected ActionResult InternalError(Exception exception)
        {
            return StatusCode(500);
        }

        protected ActionResult DisplayErrors()
        {
            var errorList = new List<string>();

            foreach (var item in ModelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    errorList.Add(error.ErrorMessage);
                }
            }

            return BadRequest(errorList);
        }

        protected bool IsUserIdSame(int userId)
        {
            return userId == UseerId;
        }
        #endregion

    }
}
