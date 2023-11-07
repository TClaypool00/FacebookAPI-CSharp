using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CoreGender
    {
        #region Private Fields
        private PostGenderViewModel _postGenderViewModel;

        #region Read-Only fields
        private readonly Gender _gender;
        #endregion
        #endregion

        #region Constructors
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

        public CoreGender(PostGenderViewModel postGenderViewModel)
        {
            Construct(postGenderViewModel);
        }
        #endregion

        #region Public Properties
        public int GenderId { get; set; }

        public string GenderName { get; set; }

        public string ProNouns { get; set; }

        public List<CoreProfile> Profiles { get; set; }
        #endregion

        #region Private methods
        private void Construct(PostGenderViewModel postGenderViewModel)
        {
            _postGenderViewModel = postGenderViewModel ?? throw new ArgumentNullException(nameof(postGenderViewModel));

            GenderName = _postGenderViewModel.GenderName;
            ProNouns = _postGenderViewModel.ProNouns;
        }
        #endregion
    }
}
