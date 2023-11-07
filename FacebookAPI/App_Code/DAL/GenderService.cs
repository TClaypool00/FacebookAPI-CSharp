using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class GenderService : ServiceHelper, IGenderService
    {
        #region Private Fields
        private readonly string _genderNameString;
        private readonly string _genderPronounsString;
        #endregion

        #region Constructors
        public GenderService(FacebookDbContext context, IConfiguration configuration) : base(configuration, context)
        {
            _tableName = _configuration.GetSection("tableNames").GetSection("gender").Value;
            _genderPronounsString = _configuration["Gender:Pronoun"];
            _genderNameString = _configuration["Gender:Name"];
        }
        #endregion

        #region Public Properties
        public string GenderCreatedOKMessage => $"{_tableName} {_addedOKMessage}";

        public string GenderNotFoundMessage => $"{_tableName} {_doesNotExistMessage}";

        public string GenderNameExistsMessage => $"{_genderNameString} {_doesNotExistMessage}";

        public string GenderPronounsExistsMessage => $"{_genderPronounsString} {_doesNotExistMessage}";

        public string GenderUpdatedOKMessage => $"{_tableName} {_updatedOKMessage}";
        #endregion

        #region Public Methods
        public async Task<CoreGender> AddGenderAsync(CoreGender coreGender)
        {
            var dataGender = new Gender(coreGender);
            await _context.Genders.AddAsync(dataGender);
            await SaveAsync();

            if (dataGender.GenderId == 0)
            {
                throw new ApplicationException($"{_tableName} {_couldNotAddedMessage}");
            }

            coreGender.GenderId = dataGender.GenderId;

            return coreGender;
        }

        public Task<bool> GenderExistsAsync(int id)
        {
            return _context.Genders.AnyAsync(g => g.GenderId == id);
        }

        public Task<bool> GenderNameExistsAsync(string genderName, int? id = null)
        {
            if (id is null)
            {
                return _context.Genders.AnyAsync(g => g.GenderName == genderName);
            }
            else
            {
                return _context.Genders.AnyAsync(g => g.GenderName == genderName && g.GenderId != id);
            }
        }

        public Task<bool> GenderPronounsExists(string pronouns, int? id = null)
        {
            if (id is null)
            {
                return _context.Genders.AnyAsync(g => g.ProNouns == pronouns);
            }
            else
            {
                return _context.Genders.AnyAsync(g => g.ProNouns == pronouns && g.GenderId != id);
            }
        }

        public async Task<CoreGender> GetGenderByIdAsync(int id)
        {
            var dataGender = await FindGenderAsync(id);

            return new CoreGender(dataGender);
        }

        public async Task<List<SelectListItem>> GetGenderDropDownAsync(int? genderId = null)
        {
            bool selected;

            var genderDropDown = new List<SelectListItem>();
            SelectListItem selectListItem;
            Gender gender;

            genderDropDown.Add(AddDefaultValue(_tableName, genderId));

            var genders = await _context.Genders.Select(g => new Gender
            {
                GenderId = g.GenderId,
                GenderName = g.GenderName
            })
            .ToListAsync();

            if (genders.Count > 0)
            {
                for (int i = 0; i < genders.Count; i++)
                {
                    gender = genders[i];

                    if (genderId.HasValue && genderId == gender.GenderId)
                    {
                        selected = true;
                    }
                    else
                    {
                        selected = false;
                    }

                    selectListItem = new SelectListItem
                    {
                        Text = gender.GenderName,
                        Value = gender.GenderId.ToString(),
                        Selected = selected
                    };

                    genderDropDown.Add(selectListItem);
                }
            }

            return genderDropDown;
        }

        public async Task<CoreGender> UpdateGenderAsync(CoreGender coreGender)
        {
            var dataGender = await FindGenderAsync(coreGender.GenderId);
            dataGender.GenderName = coreGender.GenderName;
            dataGender.ProNouns = coreGender.ProNouns;

            _context.Genders.Update(dataGender);

            await SaveAsync();

            DetachEntity(dataGender);

            return coreGender;
        }
        #endregion

        #region Private Methods
        private Task<Gender> FindGenderAsync(int id)
        {
            return _context.Genders.FirstOrDefaultAsync(g => g.GenderId == id);
        }
        #endregion
    }
}
