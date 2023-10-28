using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.Controllers
{
    public class ControllerHelper : ControllerBase
    {
        #region Private fields
        private readonly string _errorMessage;
        #endregion

        #region Protected fields
        protected readonly IConfiguration _configuration;
        #endregion

        #region Constructors
        public ControllerHelper(IConfiguration configuration) : base()
        {
            _configuration = configuration;
            _errorMessage = _configuration.GetSection("Messages").GetSection("Internal").Value;
        }
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
        #endregion
    }
}
