using System;
using System.Collections.Generic;
using System.Linq;

namespace employee_benefits_api.Classes
{
    public class Employee : Person
    {
        public int EmployeeId { get; set; }
        public double TotalCostPerYear
        {
            get
            {
                return Math.Round(TotalCostPerCheck * 26, 2);
            }
        }
        public double TotalCostPerCheck
        {
            get
            {
                return Math.Round(EmployeeCostPerCheck + Dependents.Sum(dependent => dependent.DependentCostPerCheck), 2);
            }
        }
        public double EmployeeCostPerCheck
        {
            get
            {
                //normal cost of benefits
                var normalCost = 1000.0 / 26.0;
                //if the first letter of the first name is a, apply a 10% discount. 
                return FirstName[0].ToString().ToLower() == "a" ? Math.Round(normalCost*0.1, 2) : Math.Round(normalCost,2);
            }
        }
        public List<Dependent> Dependents { get; set; }
    }
}
