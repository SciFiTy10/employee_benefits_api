using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using employee_benefits_api.Classes;
using employee_benefits_api.Database;
using Microsoft.Data.Sqlite;

namespace employee_benefits_api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DatabaseConfig databaseConfig;

        public EmployeeService(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<EmployeeList> CreateEmployee(Employee employee)
        {
            //set up the connection
            using var connection = new SqliteConnection($"Data Source={databaseConfig.Name}");
            //insert the employee and get the id generated from it's insertion
            var result = await connection.ExecuteScalarAsync<int>("INSERT INTO Employee (FirstName, LastName, Email, PhoneNumber, AddressLine1, AddressLine2, City, State, Zip)" +
                "VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @AddressLine1, @AddressLine2, @City, @State, @Zip); SELECT CAST(SCOPE_IDENTITY() as int)", employee);
            //insert the employee's dependents
            employee.Dependents.ForEach(async dependent =>
            {
                await connection.ExecuteAsync("INSERT INTO Dependent (DependentType, EmployeeId, FirstName, LastName, Email, PhoneNumber, AddressLine1, AddressLine2, City, State, Zip)" +
                "VALUES (@DependentType, @EmployeeId, @FirstName, @LastName, @Email, @PhoneNumber, @AddressLine1, @AddressLine2, @City, @State, @Zip); SELECT CAST(SCOPE_IDENTITY() as int)",
                new { DependentType = dependent.DependentType, EmployeeId = result, FirstName = dependent.FirstName, LastName = dependent.LastName, Email = dependent.Email, PhoneNumber = dependent.PhoneNumber, AddressLine1 = dependent.AddressLine1,
                AddressLine2 = dependent.AddressLine2, City = dependent.City, State = dependent.State, Zip = dependent.Zip});
            });

            //grab and return a list of all employees and their dependents to return to the client
            var employees = (await connection.QueryAsync<Employee>("SELECT rowid as EmployeeId, FirstName, LastName, Email, PhoneNumber, AddressLine1, AddressLine2, City, State, Zip FROM Employee")).AsList();

            //loop over each of the employees
            employees.ForEach(async employee =>
            {
                employee.Dependents = (await connection.QueryAsync<Dependent>("SELECT rowid as DependentId, DependentType, FirstName, LastName, Email, PhoneNumber, AddressLine1, AddressLine2, City, State, Zip FROM Dependent WHERE EmployeeId = @EmployeeId", new { EmployeeId = employee.EmployeeId })).AsList();
            });

            //return the employees
            return new EmployeeList() { Employees = employees };

        }
        public async Task<EmployeeList> GetEmployeeList()
        {
            using var connection = new SqliteConnection($"Data Source={databaseConfig.Name}");

            //grab and return a list of all employees and their dependents to return to the client
            var employees = (await connection.QueryAsync<Employee>("SELECT rowid as EmployeeId, FirstName, LastName, Email, PhoneNumber, AddressLine1, AddressLine2, City, State, Zip FROM Employee")).AsList();

            //loop over each of the employees
            employees.ForEach(async employee =>
            {
                employee.Dependents = (await connection.QueryAsync<Dependent>("SELECT rowid as DependentId, DependentType, FirstName, LastName, Email, PhoneNumber, AddressLine1, AddressLine2, City, State, Zip FROM Dependent WHERE EmployeeId = @EmployeeId", new { EmployeeId = employee.EmployeeId })).AsList();
            });

            //return the employees
            return new EmployeeList() { Employees = employees };

            /*
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
            //return employeeList;
            */
        }
    }
}
