using System;
using System.Collections.Generic;
using System.Linq;
namespace employee_benefits_api.Classes
{
    public class EmployeeList
    {
        public double GrandTotal { get
            {
                //sum the CostPerCheck from employees and their dependents
                return Employees.Sum(employee => employee.CostPerCheck + employee.Dependents.Sum(dependent => dependent.CostPerCheck));
            }
        }
        public List<Employee> Employees { get; set; }
    }
}
