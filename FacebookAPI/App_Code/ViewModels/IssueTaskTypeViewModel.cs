using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels
{
    public class IssueTaskTypeViewModel
    {
        private readonly CoreIssueTaskType _coreIssueTaskType;

        public IssueTaskTypeViewModel()
        {

        }

        public IssueTaskTypeViewModel(CoreIssueTaskType coreIssueTaskType)
        {
            if (coreIssueTaskType is null)
            {
                throw new ArgumentNullException(nameof(coreIssueTaskType));
            }

            _coreIssueTaskType = coreIssueTaskType;

            TypeId = _coreIssueTaskType.TypeId;
            TypeName = _coreIssueTaskType.TypeName;
        }

        [Display(Name = "#")]
        public int TypeId { get; set; }

        [Display(Name = "Type name")]
        [Required(ErrorMessage = "Type name is required")]
        public string TypeName { get; set; }
    }
}
