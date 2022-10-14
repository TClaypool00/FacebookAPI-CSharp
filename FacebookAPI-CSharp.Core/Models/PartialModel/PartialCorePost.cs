using FacebookAPI.Core.Models.CoreModels;

namespace FacebookAPI.Core.Models.PartialModel
{
    public class PartialCorePost
    {
        public string Body { get; set; }
        public DateTime DatePosted { get; set; }
        public int UserId { get; set; }
        public CoreUser User { get; set; }
    }
}
