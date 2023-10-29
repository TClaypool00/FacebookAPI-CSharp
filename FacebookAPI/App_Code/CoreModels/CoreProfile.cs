using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.ViewModels;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CoreProfile
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly PostUserProfileViewModel _postUserProfileViewModel;
        private readonly CoreUser _coreUser;
        private readonly Profile _profile;
        private string _middleName;

        private string _aboutMe;

        public CoreProfile(Profile profile)
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            _profile = profile;

            ProfileId = _profile.ProfileId;
            _aboutMe = _profile.AboutMe;
            BirthDate = _profile.BirthDate;
            MiddleName = _profile.MiddleName;
            
            if (_profile.Gender is not null)
            {
                Gender = new CoreGender(_profile.Gender);
                GenderId = _profile.Gender.GenderId;
            }
        }

        public CoreProfile()
        {

        }

        public CoreProfile(RegisterViewModel registerViewModel, CoreUser coreUser)
        {
            if (registerViewModel is null)
            {
                throw new ArgumentNullException(nameof(registerViewModel));
            }

            if (coreUser is null)
            {
                throw new ArgumentNullException(nameof(coreUser));
            }

            _registerViewModel = registerViewModel;

            BirthDate = (DateTime)_registerViewModel.BirthDate;
            GenderId = (int)_registerViewModel.GenderId;
            _coreUser = coreUser;
            User = _coreUser;
        }

        public CoreProfile(PostUserProfileViewModel postUserProfileViewModel)
        {
            _postUserProfileViewModel = postUserProfileViewModel ?? throw new ArgumentNullException(nameof(postUserProfileViewModel));

            ProfileId = _postUserProfileViewModel.ProfileId;
            AboutMe = _postUserProfileViewModel.AboutMe;
            BirthDate = _postUserProfileViewModel.BirthDateDate;
            MiddleName = _postUserProfileViewModel.MiddleName;
            GenderId = _postUserProfileViewModel.GenderId;
        }

        public int ProfileId { get; set; }

        public string AboutMe
        {
            get
            {
                if (_aboutMe is null)
                {
                    return "";
                }
                else
                {
                    return _aboutMe;
                }
            }
            set
            {
                _aboutMe = value;
            }
        }

        public DateTime BirthDate { get; set; }

        public string BirthDateString
        {
            get
            {
                return $"{BirthDate:yyyy}-{BirthDate:MM}-{BirthDate:dd}";
            }
        }

        public string MiddleName
        {
            get
            {
                if (_middleName is null)
                {
                    return "";
                }
                else
                {
                    return _middleName;
                }
            }

            set
            {
                _middleName = value;
            }
        }

        public int GenderId { get; set; }
        public CoreGender Gender { get; set; }

#nullable enable
        public CoreUser? User { get; set; }

        //TODO: Create CoreJobs and CoreInterests
    }
}
