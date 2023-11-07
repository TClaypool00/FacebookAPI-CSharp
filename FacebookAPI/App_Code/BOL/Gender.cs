using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    public class Gender
    {
        #region Private fields
        private readonly CoreGender _coreGender;
        #endregion

        #region Constructors
        public Gender()
        {
            
        }

        public Gender(CoreGender coreGender)
        {
            _coreGender = coreGender ?? throw new ArgumentNullException(nameof(coreGender));

            if (_coreGender.GenderId > 0)
            {
                GenderId = _coreGender.GenderId;
            }

            GenderName = _coreGender.GenderName;
            ProNouns = _coreGender.ProNouns;
        }
        #endregion

        #region Public Properties
        [Key]
        public int GenderId { get; set; }

        [Required]
        [MaxLength(255)]
        public string GenderName { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProNouns { get; set; }

        public List<Profile> Profiles { get; set; }
        #endregion
    }
}
