using WorkSphere.DepartmentService.DTOs;
using WorkSphere.DepartmentService.Models;
using WorkSphere.DepartmentService.Repositories;

namespace WorkSphere.DepartmentService.Services
{
    public class DepartmentService
    {
        private readonly DepartmentRepository _repository;

        public DepartmentService(DepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Department?> GetByNameAsync(string name)
        {
            return await _repository.GetByNameAsync(name);
        }
        public async Task<Department> CreateAsync(CreateDepartmentDto dto)
        {
            var department = new Department
            {
                Name = dto.Name,
                Description = dto.Description
            };

            return await _repository.CreateAsync(department);
        }

        public async Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
                return false;

            department.Name = dto.Name;
            department.Description = dto.Description;

            await _repository.UpdateAsync(department);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
                return false;

            await _repository.DeleteAsync(department);

            return true;
        }
    }
}
