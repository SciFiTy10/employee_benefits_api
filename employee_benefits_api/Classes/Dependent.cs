using System;
namespace employee_benefits_api.Classes
{
    public class Dependent : Person
    {
        public int DependentId { get; set; }
        public double CostPerCheck {
            get
            {
                //normal cost of benefits
                var normalCost = 500.0 / 26.0;
                //if the first letter of the first name is a, apply a 10% discount
                return FirstName[0].ToString().ToLower() == "a" ? normalCost * 0.1 : normalCost;
            }
        }
    }
}
