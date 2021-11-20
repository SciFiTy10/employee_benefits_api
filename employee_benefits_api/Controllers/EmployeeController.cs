using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using employee_benefits_api.Classes;
using employee_benefits_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace employee_benefits_api.Controllers
{
    [ApiController]
    [Route("api/Employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly IValidateEmployeeService validateEmployeeService;


        public EmployeeController(IEmployeeService _employeeService, IValidateEmployeeService _validateEmployeeService)
        {
            employeeService = _employeeService;
            validateEmployeeService = _validateEmployeeService;
        }

        /// <summary>
        /// Returns the list of employees with their dependents and associated cost per check
        /// </summary>
        [HttpGet]
        public async Task<EmployeeList> GetEmployeeList()
        {
            return await employeeService.GetEmployeeList();
        }

        /// <summary>
        /// Adds a new employee and their dependents
        /// </summary>
        /// <returns>
        /// The list of employees with their dependents and associated cost per check
        /// </returns>
        [HttpPost("CreateEmployee")]
        public async Task<CreateEmployeeResult> CreateEmployee([FromBody]Employee employee)
        {
            //validate the employee
            var result = await validateEmployeeService.ValidateEmployee(employee);
            //if successful
            if (result.Success)
            {
                return await employeeService.CreateEmployee(employee);
            }
            else
            {
                return new CreateEmployeeResult()
                {
                    Success = false,
                    Message = result.Message,
                    EmployeeList = new EmployeeList()
                };
            }
        }
    }
}
