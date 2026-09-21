using System.Net.Http.Json;

namespace WorkSphere.EmployeeService.Clients
{
    public class DepartmentClient
    {
        private readonly HttpClient _httpClient;

        public DepartmentClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DepartmentExistsAsync(string departmentName)
        {
            var response = await _httpClient.GetAsync(
                $"api/Department/by-name/{departmentName}"
            );

            return response.IsSuccessStatusCode;
        }
    }
}
