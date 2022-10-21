using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App.Models.PostModels
{
    public class ApiPostFriend
    {
        [Required(ErrorMessage = "Sender Id is required")]
        public int SenderId { get; set; }

        [Required(ErrorMessage = "Receiver id is required")]
        public int ReceiverId { get; set; }

        [JsonIgnore]
        public string IdsCannotBeSame { get; } = "You cannot be friends with yourself :)";

        public bool SenderReciverIdSame()
        {
            return SenderId == ReceiverId;
        }

        public bool IdsAreZero()
        {
            return SenderId == 0 || ReceiverId == 0;
        }
    }
}
