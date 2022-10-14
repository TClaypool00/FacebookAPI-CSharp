using FacebookAPI.App.Models.PartialModels;

namespace FacebookAPI.App.Models.ApiModels
{
    public class ApiPostModel : PartialTextModel
    {
        public int PostId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
