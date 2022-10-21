namespace FacebookAPI.Core.Models.CoreModels
{
    public class CoreFriend
    {

        public int SendId { get; set; }
        public CoreUser Sender { get; set; }

        public int ReceiverId { get; set; }
        public CoreUser Receiver { get; set; }

        public DateTime? DateAccepted { get; set; }
        public bool IsAccepted { get; set; }
    }
}