using Microsoft.AspNetCore.Mvc;
using WorkSphere.EmployeeService.Common;
using WorkSphere.EmployeeService.DTOs;
using WorkSphere.EmployeeService.Models;
using WorkSphere.EmployeeService.Services;

namespace WorkSphere.EmployeeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
       

        public EmployeeController(
            IEmployeeService service)
        {
            _service = service;
           
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _service.GetAllAsync();

            var response = new ApiResponse<List<Employee>>(
                true,
                200,
                "Employees retrieved successfully.",
                employees
            );

            return Ok(response);
        }





        


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _service.GetByIdAsync(id);

            var response = new ApiResponse<Employee>(
                true,
                200,
                "Employee retrieved successfully.",
                employee
            );

            return Ok(response);
        }


      [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            var employee = await _service.CreateAsync(dto);

            var response = new ApiResponse<Employee>(
                true,
                201,
                "Employee created successfully.",
                employee
            );

            return CreatedAtAction(
                nameof(GetById),
                new { id = employee.Id },
                response
            );
        }
      

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateEmployeeDto dto)
        {
            var employee = await _service.UpdateAsync(id, dto);

            var response = new ApiResponse<Employee>(
                true,
                200,
                "Employee updated successfully.",
                employee
            );

            return Ok(response);
        }

        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            var response = new ApiResponse<object>(
                true,
                200,
                "Employee deleted successfully.",
                null
            );

            return Ok(response);
        }
      
    }
}