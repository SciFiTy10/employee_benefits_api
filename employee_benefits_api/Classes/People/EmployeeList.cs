using System;
using System.Collections.Generic;
using System.Linq;
namespace employee_benefits_api.Classes
{
    public class EmployeeList
    {
        public double GrandTotalPerYear
        {
            get
            {
                return Math.Round(GrandTotalPerCheck * 26, 2);
            }
        }
        public double GrandTotalPerCheck {
            get
            {
                //sum the CostPerCheck from employees and their dependents
                return Math.Round(Employees.Sum(employee => employee.TotalCostPerCheck),2);
            }
        }
        public List<Employee> Employees { get; set; }
    }
}
