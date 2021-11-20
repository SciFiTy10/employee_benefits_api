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
        public async Task<ActionResult> CreateEmployee([FromBody]Employee employee)
        {
            //validate the employee
            var validationResult = await validateEmployeeService.ValidateEmployee(employee);
            //if validation was successful
            if (validationResult.Success)
            {
                //create the employee
                var createEmployeeResult = await employeeService.CreateEmployee(employee);
                //if the employee was created
                if (createEmployeeResult.Success)
                {
                    //get the latest employee list
                    var employeeList = await employeeService.GetEmployeeList();
                    //return the result
                    return new CreateEmployeeResult()
                    {
                        Success = true,
                        Message = "Success! Employee was added.",
                        EmployeeList = employeeList
                    };
                }
                else
                {
                    //return the error message
                    return createEmployeeResult;
                }
            }
            else
            {
                //return the error message
                return validationResult;
            }
        }
    }
}
