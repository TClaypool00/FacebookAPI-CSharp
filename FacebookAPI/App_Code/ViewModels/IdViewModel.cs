using FacebookAPI.App_Code.CustomAnnotation;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels
{
    public class IdViewModel
    {
        [Required(ErrorMessage = "Id is required")]
        [IdMustBeGreaterThanZero]
        public int Id { get; set; }
    }
}
