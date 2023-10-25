using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    //TODO: Add Service for Profiles
    //TODO: Get and get all should come from the User Service
    public class Profile
    {
        private readonly CoreProfile _coreProfile;

        public Profile()
        {

        }

        public Profile(CoreProfile coreProfile)
        {
            if (coreProfile is null)
            {
                throw new ArgumentNullException(nameof(coreProfile));
            }

            _coreProfile = coreProfile;

            if (_coreProfile.ProfileId > 0)
            {
                ProfileId = _coreProfile.ProfileId;
            }

            BirthDate = _coreProfile.BirthDate;
            GenderId = _coreProfile.GenderId;
            MiddleName = _coreProfile.MiddleName;
            AboutMe = _coreProfile.AboutMe;
        }

        [Key]
        public int ProfileId { get; set; }

        [MaxLength(255)]
        public string AboutMe { get; set; }

        [MaxLength(255)]
        public string MiddleName { get; set; }

        public DateTime BirthDate { get; set; }

        public int GenderId { get; set; }
        public Gender Gender { get; set; }

#nullable enable
        public User? User { get; set; }

#nullable disable
        public List<Job> Jobs { get; set; }
        public List<Interest> Interests { get; set; }
    }
}
