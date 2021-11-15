using System.Threading.Tasks;
using employee_benefits_api.Classes;

namespace employee_benefits_api.Services
{
    public interface IValidateEmployeeService
    {
        Task<ValidationResult> ValidateEmployee(Employee employee);
    }
}