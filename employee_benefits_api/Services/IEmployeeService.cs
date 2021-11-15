using System;
using System.Threading.Tasks;
using employee_benefits_api.Classes;

namespace employee_benefits_api.Services
{
    /// <summary>
    /// Returns the list of employees with their dependents and associated cost per check
    /// </summary>
    public interface IEmployeeService
    {
        Task<EmployeeList> GetEmployeeList();
        Task<EmployeeList> CreateEmployee(Employee employee);
    }
}
