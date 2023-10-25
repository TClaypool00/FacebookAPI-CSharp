using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacebookAPI.App_Code.BLL
{
    public interface IGenderService
    {
        public Task<List<SelectListItem>> GetGenderDropDownAsync(int? genderId = null);
    }
}
