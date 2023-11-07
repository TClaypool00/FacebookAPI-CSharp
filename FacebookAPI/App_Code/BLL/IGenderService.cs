using FacebookAPI.App_Code.CoreModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacebookAPI.App_Code.BLL
{
    public interface IGenderService
    {
        #region Public Methods
        public Task<List<SelectListItem>> GetGenderDropDownAsync(int? genderId = null);

        public Task<List<CoreGender>> GetGendersAsync();

        public Task<CoreGender> AddGenderAsync(CoreGender coreGender);

        public Task<CoreGender> UpdateGenderAsync(CoreGender coreGender);

        public Task<CoreGender> GetGenderByIdAsync(int id);

        public Task<bool> GenderNameExistsAsync(string genderName, int? id = null);

        public Task<bool> GenderPronounsExists(string pronouns,  int? id = null);

        public Task<bool> GenderExistsAsync(int id);
        #endregion

        #region Public Proprties
        public string GenderCreatedOKMessage { get; }

        public string GenderUpdatedOKMessage { get; }

        public string GenderNotFoundMessage { get; }

        public string GenderNameExistsMessage { get; }

        public string GenderPronounsExistsMessage { get; }

        public string GendersNotFoundMessage { get; }
        #endregion
    }
}
