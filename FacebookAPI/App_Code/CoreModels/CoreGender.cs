using FacebookAPI.App_Code.BOL;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CoreGender
    {
        private readonly Gender _gender;

        public CoreGender()
        {

        }

        public CoreGender(Gender gender)
        {
            if (gender is null)
            {
                throw new ArgumentNullException(nameof(gender));
            }

            _gender = gender;

            GenderId = _gender.GenderId;
            GenderName = _gender.GenderName;
            ProNouns = _gender.ProNouns;
        }

        public int GenderId { get; set; }

        public string GenderName { get; set; }

        public string ProNouns { get; set; }

        public List<CoreProfile> Profiles { get; set; }
    }
}
