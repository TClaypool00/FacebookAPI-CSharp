using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.ViewModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CoreIssueTaskType
    {
        private readonly IssueTaskType _taskType;
        private readonly IssueTaskTypeViewModel _issueTaskTypeViewModel;

        public CoreIssueTaskType()
        {

        }

        public CoreIssueTaskType(IssueTaskTypeViewModel issueTask)
        {
            if (issueTask is null)
            {
                throw new ArgumentNullException(nameof(issueTask));
            }

            _issueTaskTypeViewModel = issueTask;

            TypeId = _issueTaskTypeViewModel.TypeId;
            TypeName = _issueTaskTypeViewModel.TypeName;
        }

        public CoreIssueTaskType(IssueTaskType taskType)
        {
            if (taskType is null)
            {
                throw new ArgumentNullException(nameof(taskType));
            }

            _taskType = taskType;

            TypeId = _taskType.TypeId;
            TypeName = _taskType.TypeName;
        }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public List<CoreIssueTask> IssueTasks { get; set; }
    }
}
