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

        public EmployeeController(IEmployeeService _employeeService)
        {
            employeeService = _employeeService;
        }

        /// <summary>
        /// Returns the list of employees with their dependents and associated cost per check
        /// </summary>
        [HttpGet]
        public async Task<EmployeeList> GetEmployeeList()
        {
            return await employeeService.GetEmployeeList();
        }
    }
}
