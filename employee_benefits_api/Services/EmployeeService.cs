using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using employee_benefits_api.Classes;

namespace employee_benefits_api.Services
{
    public class EmployeeService : IEmployeeService
    {
        public async Task<EmployeeList> GetEmployeeList()
        {
            //create a temp list of employees
            EmployeeList employeeList = new EmployeeList()
            {
                Employees = new List<Employee>()
                {
                    new Employee()
                    {
                        EmployeeId = 1,
                        FirstName = "Pat",
                        LastName = "Johnson",
                        Email = "pjohnson@gmail.com",
                        PhoneNumber = 5555555555,
                        AddressLine1 = "123 Fourth St",
                        AddressLine2 = "Apt. 9",
                        City = "Crystal Lake",
                        State = "Illinois",
                        Zip = 60014,
                        Dependents = new List<Dependent>()
                        {
                            new Dependent()
                            {
                                DependentId = 1,
                                DependentType = "Spouse",
                                FirstName = "Jaimie",
                                LastName = "Johnson",
                                Email = "jjohnson@gmail.com",
                                PhoneNumber = 6666666666,
                                AddressLine1 = "123 Fourth St",
                                AddressLine2 = "Apt. 9",
                                City = "Crystal Lake",
                                State = "Illinois",
                                Zip = 60014
                            },
                            new Dependent()
                            {
                                DependentId = 2,
                                DependentType = "Child",
                                FirstName = "Jimmy",
                                LastName = "Johnson",
                                Email = "",
                                PhoneNumber = 0,
                                AddressLine1 = "123 Fourth St",
                                AddressLine2 = "Apt. 9",
                                City = "Crystal Lake",
                                State = "Illinois",
                                Zip = 60014
                            }
                        }
                    }
                }
            };
            return employeeList;
        }
    }
}
