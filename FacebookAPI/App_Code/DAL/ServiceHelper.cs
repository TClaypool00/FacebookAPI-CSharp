using FacebookAPI.App_Code.BOL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class ServiceHelper
    {
        protected int _index;
        protected readonly int _takeValue;
        protected readonly int _subTakeValue;
        protected readonly IConfiguration _configuration;
        protected readonly int _longerTakeValue;
        protected readonly FacebookDbContext _context;

        public ServiceHelper(IConfiguration configuration, FacebookDbContext context)
        {
            _configuration = configuration;
            _takeValue = int.Parse(_configuration.GetSection("app").GetSection("standardTakeValue").Value);
            _subTakeValue = int.Parse(_configuration.GetSection("app").GetSection("standardTakeValue").Value);
            _longerTakeValue = int.Parse(_configuration.GetSection("app").GetSection("longerTakeValue").Value);
            _context = context;
        }

        protected void ConfigureIndex(int? index)
        {
            if (!index.HasValue || index == 0)
            {
                _index = 0;
            }
            else
            {
                _index = _takeValue * index.Value;
            }
        }

        protected SelectListItem AddDefaultValue(string model, int? id = null)
        {
            bool selected;

            if (id.HasValue)
            {
                selected = false;
            }
            else
            {
                selected = true;
            }

            return new SelectListItem
            {
                Value = "",
                Text = $"Please select a {model}...",
                Selected = selected
            };
        }

        protected async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected void DetachEntity(object model)
        {
            if (model != null)
            {
                _context.Entry(model).State = EntityState.Detached;
            }
        }
    }
}
