using System;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;

namespace employee_benefits_api.Database
{
    public class DatabaseBootstrap : IDatabaseBootstrap
    {
        private readonly DatabaseConfig databaseConfig;

        public DatabaseBootstrap(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public void SetUp()
        {
            //set up the database connection
            using var connection = new SqliteConnection($"Data Source={databaseConfig.Name}");

            //query for the employee table
            var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Employee';");
            var tableName = table.FirstOrDefault();
            //if the table exists
            if (!string.IsNullOrEmpty(tableName) && tableName == "Employee")
            {
                return;
            }
            try
            {
                //otherwise create the Employee table
                connection.Execute("Create Table Employee (" +
                    " FirstName VARCHAR(50) NOT NULL, " +
                    " LastName VARCHAR(50) NOT NULL, " +
                    " Email VARCHAR(50) NULL, " +
                    " PhoneNumber BIGINT NULL, " +
                    " AddressLine1 VARCHAR(200) NOT NULL, " +
                    " AddressLine2 VARCHAR(200) NOT NULL, " +
                    " City VARCHAR(100) NOT NULL, " +
                    " State VARCHAR(25) NOT NULL, " +
                    " Zip INT NOT NULL);"
                );

                //query for the dependent table
                var dependentTable = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Dependent';");
                var dependentTableName = dependentTable.FirstOrDefault();
                //if the table exists
                if (!string.IsNullOrEmpty(tableName) && tableName == "Dependent")
                {
                    return;
                }
                //otherwise create the Employee table
                connection.Execute("Create Table Dependent (" +
                    " DependentType VARCHAR(25) NOT NULL, " +
                    " EmployeeId INT NOT NULL, " +
                    " FirstName VARCHAR(50) NOT NULL, " +
                    " LastName VARCHAR(50) NOT NULL, " +
                    " Email VARCHAR(50) NULL, " +
                    " PhoneNumber BIGINT NULL, " +
                    " AddressLine1 VARCHAR(200) NOT NULL, " +
                    " AddressLine2 VARCHAR(200) NOT NULL, " +
                    " City VARCHAR(100) NOT NULL, " +
                    " State VARCHAR(25) NOT NULL, " +
                    " Zip INT NOT NULL);"
                );
            }
            catch(Exception e)
            {
                var ex = e;
            }
        }
    }
}
