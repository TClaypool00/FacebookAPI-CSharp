namespace FacebookAPI.DataAccess.Models
{
    public class Friend
    {
        public int SendId { get; set; }
        public User Sender { get; set; }

        public int ReceiverId { get; set; }
        public User Receiver { get; set; }

        public DateTime? DateAccepted { get; set; } = null;
        public bool IsAccepted { get; set; } = false;
    }
}
