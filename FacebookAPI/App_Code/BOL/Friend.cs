using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    public class Friend
    {
        private int _senderId;
        private int _receiverId;
        private readonly CoreFriend _friend;

        public Friend(CoreFriend friend)
        {
            if (friend is null)
            {
                throw new ArgumentNullException(nameof(friend));
            }

            _friend = friend;
            FriendId = _friend.FriendId;
            SenderId = _friend.SenderId;
            ReceiverId = _friend.ReceiverId;
            DateAccepted = _friend.DateAccepted;
        }

        public Friend()
        {

        }

        public Friend(int senderId, int receiverId)
        {
            _senderId = senderId;
            _receiverId = receiverId;
        }

        [Key]
        public int FriendId { get; set; }

        public int SenderId
        {
            get { return _senderId; }
            set
            {
                _senderId = value;
            }
        }
        public User Sender { get; set; }

        public int ReceiverId
        {
            get { return _receiverId; }
            set { _receiverId = value; }
        }
        public User Receiver { get; set; }

        public DateTime? DateAccepted { get; set; }

        public enum FriendTypes
        {
            NotFriends,
            Pending,
            Friends
        }

        public enum UserFriendTypes
        {
            UserIsSender,
            UserIsReceiver
        }
    }
}
