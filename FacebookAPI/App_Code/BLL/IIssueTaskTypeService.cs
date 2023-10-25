using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IIssueTaskTypeService
    {
        #region Public Method
        public Task<List<CoreIssueTaskType>> GetIssueTaskTypesAsync(int? index = null);

        public Task<CoreIssueTaskType> AddIssueTaskType(CoreIssueTaskType issueTaskType);

        public Task<bool> IssueTaskTypeExistsAsync(string typeName, int? id = null);
        #endregion

        #region Messages
        public string IssueTaskTypeSuccessMessage { get; }

        public string IssueTaskTypeAlreadyExistsMessage { get; }
        #endregion
    }
}
