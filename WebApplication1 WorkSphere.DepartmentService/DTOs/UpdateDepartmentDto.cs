using System.ComponentModel.DataAnnotations;

namespace WorkSphere.DepartmentService.DTOs
{
    public class UpdateDepartmentDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}

