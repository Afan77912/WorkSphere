using System.ComponentModel.DataAnnotations;
using WorkSphere.EmployeeService.DTOs;

namespace WorkSphere.EmployeeService.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CreateEmployeeDto_WithValidData_ShouldPassValidation()
        {
            var dto = new CreateEmployeeDto
            {
                Name = "Afan Dalvi",
                Department = "IT",
                Age = 24,
                Email = "afan@example.com",
                Phone = "9876543210"
            };

            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(
                dto,
                context,
                results,
                validateAllProperties: true);

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void CreateEmployeeDto_WithInvalidAge_ShouldFailValidation()
        {
            var dto = new CreateEmployeeDto
            {
                Name = "Afan Dalvi",
                Department = "IT",
                Age = 15,
                Email = "afan@example.com",
                Phone = "9876543210"
            };

            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(
                dto,
                context,
                results,
                validateAllProperties: true);

            Assert.False(isValid);
            Assert.Contains(results, r =>
                r.MemberNames.Contains(nameof(CreateEmployeeDto.Age)));
        }
    }
}