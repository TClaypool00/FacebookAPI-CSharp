namespace FacebookAPI.App_Code.CoreModels
{
    public class CoreIssueTask
    {
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int TypeId { get; set; }
        public CoreIssueTaskType Type { get; set; }

        public int UserId { get; set; }
        public CoreUser User { get; set; }
    }
}
