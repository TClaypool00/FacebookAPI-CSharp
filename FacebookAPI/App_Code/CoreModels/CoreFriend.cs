using FacebookAPI.App_Code.BOL;
using static FacebookAPI.App_Code.BOL.Friend;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CoreFriend
    {
        public CoreFriend()
        {

        }

        public CoreFriend(Friend friend)
        {
            if (friend is null)
            {
                throw new ArgumentNullException(nameof(friend));
            }

            Construct(friend);

            FriendTypes = FriendTypes.NotFriends;
            UserFriendType = UserFriendTypes.UserIsSender;
        }

        public CoreFriend(Friend friend, int userId)
        {
            if (friend is null)
            {
                FriendTypes = FriendTypes.NotFriends;
                return;
            }
            _userId = userId;

            Construct(friend);

            if (DateAccepted is null)
            {
                FriendTypes = FriendTypes.Pending;
            }
            else
            {
                FriendTypes = FriendTypes.Friends;
            }

            if (_userId == SenderId)
            {
                UserFriendType = UserFriendTypes.UserIsSender;
            }
            else
            {
                UserFriendType = UserFriendTypes.UserIsReceiver;
            }
        }

        private Friend _friend;

        private readonly int _userId;

        private DateTime? _dateAccepted;

        public int FriendId { get; set; }

        public int SenderId { get; set; }
        public CoreUser User { get; set; }

        public int ReceiverId { get; set; }
        public CoreUser Reciever { get; set; }

        public FriendTypes FriendTypes { get; set; }

        public UserFriendTypes UserFriendType { get; set; }

        public DateTime? DateAccepted
        {
            get
            {
                return _dateAccepted;
            }
            set
            {
                _dateAccepted = value;
            }
        }
        public string DateAcceptedString
        {
            get
            {
                if (_dateAccepted.HasValue)
                {
                    return _dateAccepted.Value.ToString("f");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private void Construct(Friend friend)
        {
            _friend = friend;

            FriendId = _friend.FriendId;
            SenderId = _friend.SenderId;
            ReceiverId = _friend.ReceiverId;
            DateAccepted = _friend.DateAccepted;
        }
    }
}
