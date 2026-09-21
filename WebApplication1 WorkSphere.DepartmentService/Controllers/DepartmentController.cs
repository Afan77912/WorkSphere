using Microsoft.AspNetCore.Mvc;
using WorkSphere.DepartmentService.DTOs;
using WorkSphere.DepartmentService.Services;

namespace WebApplication1_WorkSphere.DepartmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly WorkSphere.DepartmentService.Services.DepartmentService _service;

        public DepartmentController(
            WorkSphere.DepartmentService.Services.DepartmentService service)
        {
            _service = service;
        }
       
        // GET: api/Department
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _service.GetAllAsync();

            return Ok(departments);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var department = await _service.GetByNameAsync(name);

            if (department == null)
                return NotFound();

            return Ok(department);
        }

        // GET: api/Department/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _service.GetByIdAsync(id);

            if (department == null)
                return NotFound();

            return Ok(department);
        }

        // POST: api/Department
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            var department = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = department.Id },
                department
            );
        }

        // PUT: api/Department/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateDepartmentDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Department/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
