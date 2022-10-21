namespace FacebookAPI.App.Models.ApiModels
{
    public class ApiFriend
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateAccepted { get; set; }
        public bool IsAccepted { get; set; }
    }
}
