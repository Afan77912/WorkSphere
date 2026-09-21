using System.ComponentModel.DataAnnotations;

namespace WorkSphere.EmployeeService.DTOs
{
    public class UpdateEmployeeDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Department { get; set; } = string.Empty;

        [Range(18, 60)]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
    }
}
