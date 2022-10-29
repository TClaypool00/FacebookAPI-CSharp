using FacebookAPI.Core.Interfaces;
using FacebookAPI.Core.Models.CoreModels;
using FacebookAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.DataAccess.Services
{
    public class ParentTypeService : IParentTypeService, IGeneralService
    {
        private readonly FacebookDBContext _context;

        public ParentTypeService(FacebookDBContext context)
        {
            _context = context;
        }

        public async Task<CoreParentType> AddParentTypeAsync(CoreParentType parentType)
        {
            var dataParentType = Mapper.MapParentType(parentType);

            await _context.ParentTypes.AddAsync(dataParentType);
            await SaveAsync();

            parentType.ParenTypeId = dataParentType.ParentTypeId;

            return parentType;
        }

        public async Task DeleteParentTypeAsync(int id)
        {
            _context.ParentTypes.Remove(await FindParentType(id));
            await SaveAsync();
        }

        public async Task<List<CoreParentType>> GetParentTypesAsync()
        {
            var dataParentTypes = await _context.ParentTypes.ToListAsync();

            return dataParentTypes.Select(Mapper.MapParentType).ToList();
        }

        public Task<bool> ParentTypeExistsAsync(int id)
        {
            return _context.ParentTypes.AnyAsync(p => p.ParentTypeId == id);
        }

        public Task<bool> ParentTypeNameExists(string name, int? id = null)
        {
            if (id is null)
            {
                return _context.ParentTypes.AnyAsync(p => p.Name == name);
            }

            return _context.ParentTypes.AnyAsync(p => p.Name == name && p.ParentTypeId != id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<CoreParentType> UpdateParentTypeAsync(int id, CoreParentType parentType)
        {
            var oldParentType = await FindParentType(id);
            var newParentType = Mapper.MapParentType(parentType);

            _context.Entry(oldParentType).CurrentValues.SetValues(newParentType);

            await SaveAsync();

            return parentType;
        }

        private async Task<ParentType> FindParentType(int id)
        {
            return await _context.ParentTypes.FirstOrDefaultAsync(p => p.ParentTypeId == id);
        }
    }
}
