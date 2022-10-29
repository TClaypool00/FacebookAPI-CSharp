using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.DataAccess.Models
{
    public class ParentType
    {
        [Key]
        public int ParentTypeId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
