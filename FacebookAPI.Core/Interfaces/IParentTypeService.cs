using FacebookAPI.Core.Models.CoreModels;

namespace FacebookAPI.Core.Interfaces
{
    public interface IParentTypeService
    {
        Task<List<CoreParentType>> GetParentTypesAsync();
        Task<CoreParentType> AddParentTypeAsync(CoreParentType parentType);
        Task<CoreParentType> UpdateParentTypeAsync(int id, CoreParentType parentType);
        Task<bool> ParentTypeExistsAsync(int id);
        Task<bool> ParentTypeNameExists(string name, int? id = null);
        Task DeleteParentTypeAsync(int id);
    }
}