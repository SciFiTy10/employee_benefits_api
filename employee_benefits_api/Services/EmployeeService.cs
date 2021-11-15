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
            try
            {
                //set up the connection
                using var connection = new SqliteConnection($"Data Source={databaseConfig.Name}");
                //insert the employee and get the id generated from it's insertion
                var result = await connection.ExecuteScalarAsync<int>("INSERT INTO Employee (FirstName, LastName, Email, PhoneNumber, AddressLine1, AddressLine2, City, State, Zip)" +
                    "VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @AddressLine1, @AddressLine2, @City, @State, @Zip); SELECT last_insert_rowid()", employee);
                //insert the employee's dependents
                employee.Dependents.ForEach(async dependent =>
                {
                    await connection.ExecuteAsync("INSERT INTO Dependent (DependentType, EmployeeId, FirstName, LastName, Email, PhoneNumber, AddressLine1, AddressLine2, City, State, Zip)" +
                    "VALUES (@DependentType, @EmployeeId, @FirstName, @LastName, @Email, @PhoneNumber, @AddressLine1, @AddressLine2, @City, @State, @Zip); SELECT last_insert_rowid()",
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
            catch(Exception e)
            {
                var ex = e;
                return new EmployeeList();
            }

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
        }
    }
}
