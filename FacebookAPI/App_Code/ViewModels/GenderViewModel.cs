using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.ViewModels
{
    public class GenderViewModel : PostGenderViewModel
    {
        #region Private fields
        private CoreGender _coreGender;
        #endregion

        #region Constructors
        public GenderViewModel()
        {
            
        }

        public GenderViewModel(CoreGender coreGender)
        {
            Construct(coreGender);

            Message = "";
        }

        public GenderViewModel(CoreGender coreGender, string message)
        {
            Construct(coreGender);

            Message = message;
        }
        #endregion

        #region Public Propeties
        public int GenderId { get; set; }

        public string Message { get; set; }
        #endregion

        #region Private Methods
        private void Construct(CoreGender coreGender)
        {
            _coreGender = coreGender ?? throw new ArgumentNullException(nameof(coreGender));
            GenderId = _coreGender.GenderId;
            GenderName = _coreGender.GenderName;
            ProNouns = _coreGender.ProNouns;
        }
        #endregion
    }
}
