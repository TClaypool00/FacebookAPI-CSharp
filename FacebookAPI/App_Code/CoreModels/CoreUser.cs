using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.ViewModels;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CoreUser
    {
        private readonly RegisterViewModel _registerViewModel;
        private User _user;
        private IConfiguration _configuration;
        private readonly int _currentUserId;

        private int _userId;

        public int UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                _userId = value;
            }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string ProtectedName { get; set; }

        public bool IsAdmin { get; set; }

        public bool SameUser
        {
            get
            {
                return _userId == _currentUserId;
            }
        }

        public int ProfileId { get; set; }
        public CoreProfile Profile { get; set; }

        public int PictureId { get; set; }
        public CorePicture Picture { get; set; }

        public List<CorePost> Posts { get; set; }
        public List<CoreComment> Comments { get; set; }
        public List<CoreIssueTask> IssueTasks { get; set; }
        public List<CorePicture> Pictures { get; set; }


        //TODO: Create CoreReply        

        public CoreUser()
        {

        }

        public CoreUser(User user)
        {
            Construct(user);
        }

        public CoreUser(RegisterViewModel registerViewModel)
        {
            if (registerViewModel is null)
            {
                throw new ArgumentNullException(nameof(registerViewModel));
            }

            _registerViewModel = registerViewModel;

            FirstName = _registerViewModel.FirstName;
            LastName = _registerViewModel.LastName;
            Email = _registerViewModel.Email;
            PhoneNumber = _registerViewModel.PhoneNumber;
            Password = _registerViewModel.Password;            

            Profile = new CoreProfile(_registerViewModel, this);
        }

        public CoreUser(User usr, IConfiguration configuration)
        {
            Construct(usr, configuration);
        }

        public CoreUser(User user, int currentUserId, IConfiguration configuration)
        {
            Construct(user, configuration);

            _currentUserId = currentUserId;
        }

        public void SetProperites(PostUserProfileViewModel model)
        {
            FirstName = model.FirstName;
            Profile.MiddleName = model.MiddleName;
            LastName = model.LastName;
            Email = model.Email;
            PhoneNumber= model.PhoneNumber;
            Profile.BirthDate = model.BirthDateDate;
            Profile.GenderId = model.GenderId;
            Profile.AboutMe = model.AboutMe;
        }

        private void Construct(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _user = user;

            _userId = _user.UserId;
            FirstName = _user.FirstName;
            LastName = _user.LastName;
            Email = _user.Email;
            PhoneNumber = _user.PhoneNumber;
            Password = _user.Password;
            IsAdmin = _user.IsAdmin;
            ProtectedName = $"{_user.FirstName} {_user.LastName[0]}.";

            if (_user.Profile is not null)
            {
                Profile = new CoreProfile(_user.Profile);
                ProfileId = _user.Profile.ProfileId;
            }
        }

        private void Construct(User user, IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            Construct(user);

            if (_user.ProfilePicture is null)
            {
                Picture = new CorePicture(_configuration);
            }
            else
            {
                Picture = new CorePicture(_user.ProfilePicture, _userId);
            }
        }
    }
}
