using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class GenderService : ServiceHelper, IGenderService
    {
        private readonly string _name;

        public GenderService(FacebookDbContext context, IConfiguration configuration) : base(configuration, context)
        {
            _name = _configuration.GetSection("tableNames").GetSection("gender").Value;
        }

        public async Task<List<SelectListItem>> GetGenderDropDownAsync(int? genderId = null)
        {
            bool selected;

            var genderDropDown = new List<SelectListItem>();
            SelectListItem selectListItem;
            Gender gender;

            genderDropDown.Add(AddDefaultValue(_name, genderId));

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
    }
}
