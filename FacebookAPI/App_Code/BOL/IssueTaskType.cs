using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    public class IssueTaskType
    {
        private readonly CoreIssueTaskType _type;

        public IssueTaskType()
        {

        }

        public IssueTaskType(CoreIssueTaskType type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            _type = type;

            if (_type.TypeId > 0)
            {
                TypeId = _type.TypeId;
            }

            TypeName = _type.TypeName;
        }

        [Key]
        public int TypeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string TypeName { get; set; }

        public List<IssueTask> IssueTasks { get; set; }
    }
}
