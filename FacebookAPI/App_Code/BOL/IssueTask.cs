using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    public class IssueTask
    {
        private readonly CoreIssueTask _issueTask;

        public IssueTask()
        {

        }

        public IssueTask(CoreIssueTask issueTask)
        {
            if (issueTask is null)
            {
                throw new ArgumentNullException(nameof(issueTask));
            }

            _issueTask = issueTask;

            if (_issueTask.TaskId > 0)
            {
                TaskId = _issueTask.TaskId;
            }

            TaskName = _issueTask.TaskName;
            StartDate = _issueTask.StartDate;
            EndDate = _issueTask.EndDate;
            TypeId = _issueTask.TypeId;
            UserId = _issueTask.UserId;
        }

        [Key]
        public int TaskId { get; set; }

        [Required]
        [MaxLength(400)]
        public string TaskName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public int TypeId { get; set; }
        public IssueTaskType Type { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
