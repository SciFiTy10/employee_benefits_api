using System;
namespace employee_benefits_api.Classes
{
    public class CreateEmployeeResult : ValidationResult
    {
        public EmployeeList EmployeeList { get; set; }
    }
}
