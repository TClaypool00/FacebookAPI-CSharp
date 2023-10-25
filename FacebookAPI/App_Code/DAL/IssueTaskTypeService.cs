using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class IssueTaskTypeService : ServiceHelper, IIssueTaskTypeService
    {
        public IssueTaskTypeService(IConfiguration configuration, FacebookDbContext context) : base(configuration, context)
        {
        }

        public string IssueTaskTypeSuccessMessage => "Issue Task Type has been created";

        public string IssueTaskTypeAlreadyExistsMessage => "Issue Task Type already exists";

        public async Task<CoreIssueTaskType> AddIssueTaskType(CoreIssueTaskType issueTaskType)
        {
            var dataIssueTaskType = new IssueTaskType(issueTaskType);

            await _context.IssueTaskTypes.AddAsync(dataIssueTaskType);

            await SaveAsync();

            issueTaskType.TypeId = dataIssueTaskType.TypeId;

            return issueTaskType;
        }

        public async Task<List<CoreIssueTaskType>> GetIssueTaskTypesAsync(int? index = null)
        {
            ConfigureIndex(index);

            var coreTaskTypes = new List<CoreIssueTaskType>();

            var taskTypes = await _context.IssueTaskTypes
                .Select(t => new IssueTaskType
                {
                    TypeId = t.TypeId,
                    TypeName = t.TypeName
                })
                .Take(_longerTakeValue)
                .Skip(_index)
                .ToListAsync();

            if (taskTypes.Count > 0)
            {
                for (int i = 0; i < taskTypes.Count; i++)
                {
                    coreTaskTypes.Add(new CoreIssueTaskType(taskTypes[i]));
                }
            }

            return coreTaskTypes;
        }

        public Task<bool> IssueTaskTypeExistsAsync(string typeName, int? id = null)
        {
            if (id is null)
            {
                return _context.IssueTaskTypes.AnyAsync(t => t.TypeName ==  typeName);
            }

            return _context.IssueTaskTypes.AnyAsync(t => t.TypeName == typeName && t.TypeId != id);
        }
    }
}
