using FacebookAPI.App.Models.ApiModels;
using FacebookAPI.App.Models.PostModels;
using FacebookAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FacebookAPI.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentTypesController : ControllerHelper
    {
        private readonly IParentTypeService _service;

        public ParentTypesController(IParentTypeService service)
        {
            _service = service;
        }

        // GET: api/ParentTypes
        [HttpGet]
        public async Task<ActionResult<List<ApiParentType>>> GetParentTypes()
        {
            var parentTypes = await _service.GetParentTypesAsync();

            if (parentTypes.Count > 0)
            {
                return Ok(parentTypes.Select(ApiMapper.MapParentType).ToList());
            }

            return NotFound("No parent types found");
        }

        // PUT: api/ParentTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParentTypeAsync(int id, ApiPostParentType parentType)
        {
            try
            {
                if (!await _service.ParentTypeExistsAsync(id))
                {
                    return NotFound(ParentTypeNotFound(id));
                }

                if (!await _service.ParentTypeNameExists(parentType.Name, id))
                {
                    return BadRequest(ParentTypeNameExists(parentType.Name));
                }

                var coreParentType = ApiMapper.MapParentType(parentType, id);
                coreParentType = await _service.UpdateParentTypeAsync(id, coreParentType);
                return Ok(ApiMapper.MapParentType(coreParentType));
            } catch (Exception)
            {
                return StatusCode(500, UserErrorMessage());
            }
        }

        // POST: api/ParentTypes
        [HttpPost]
        public async Task<ActionResult<ApiParentType>> PostParentType(ApiPostParentType parentType)
        {
            try
            {
                if (!await _service.ParentTypeNameExists(parentType.Name))
                {
                    return BadRequest(ParentTypeNameExists(parentType.Name));
                }

                var coreParentType = ApiMapper.MapParentType(parentType);
                coreParentType = await _service.AddParentTypeAsync(coreParentType);

                return Ok(ApiMapper.MapParentType(coreParentType));
            } catch (Exception)
            {
                return StatusCode(500, UserErrorMessage());
            }
        }

        // DELETE: api/ParentTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParentType(int id)
        {
            try
            {
                if (! await _service.ParentTypeExistsAsync(id))
                {
                    return NotFound(ParentTypeNotFound(id));
                }

                await _service.DeleteParentTypeAsync(id);

                return Ok("Parent type has been deleted");
            }
            catch (Exception)
            {
                return StatusCode(500, UserErrorMessage());
            }
        }

        private static string ParentTypeNameExists(string name)
        {
            return $"A parent type with the name {name} already exists";
        }

        private static string ParentTypeNotFound(int id)
        {
            return $"A parent type with an id of {id} could not be found";
        }
    }
}
