using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels;
using FacebookAPI.App_Code.ViewModels.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class GendersController : ControllerHelper
    {
        #region Private fields
        private readonly IGenderService _genderService;
        #endregion

        #region Constructors
        public GendersController(IConfiguration configuration, IGenderService genderService) : base(configuration)
        {
            _genderService = genderService;
        }
        #endregion

        #region Public Methods
        [HttpPost]
        public async Task<ActionResult> AddGenderAsync([FromBody] PostGenderViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return DisplayErrors();
                }

                if (await _genderService.GenderNameExistsAsync(model.GenderName))
                {
                    return BadRequest(_genderService.GenderNameExistsMessage);
                }

                if (await _genderService.GenderPronounsExists(model.ProNouns))
                {
                    return BadRequest(_genderService.GenderPronounsExistsMessage);
                }

                var coreGender = new CoreGender(model);

                coreGender = await _genderService.AddGenderAsync(coreGender);

                return Ok(new GenderViewModel(coreGender, _genderService.GenderCreatedOKMessage));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }
        #endregion
    }
}
