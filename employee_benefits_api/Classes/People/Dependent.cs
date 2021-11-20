﻿using System;
namespace employee_benefits_api.Classes
{
    public class Dependent : Person
    {
        public int DependentId { get; set; }
        public string DependentType { get; set; }
        public double DependentCostPerCheck {
            get
            {
                //normal cost of benefits
                var normalCost = 500.0 / 26.0;
                //if the first letter of the first name is a, apply a 10% discount
                return FirstName[0].ToString().ToLower() == "a" ? Math.Round(normalCost * 0.1,2) : Math.Round(normalCost,2);
            }
        }
    }
}
