using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using employee_benefits_api.Classes;

namespace employee_benefits_api.Services
{
    public class ValidateEmployeeService : IValidateEmployeeService
    {
        public ValidateEmployeeService()
        {
        }

        public async Task<ValidationResult> ValidateEmployee(Employee employee)
        {
            //validate first name
            var firstNameResult = IsItemMissing(employee.FirstName, "First Name");
            //if unsuccessful
            if (!firstNameResult.Success)
            {
                return firstNameResult;
            }
            //validate last name
            var lastNameResult = IsItemMissing(employee.LastName, "Last Name");
            //if unsuccessful
            if (!lastNameResult.Success)
            {
                return lastNameResult;
            }
            //validate email
            var emailResult = IsItemMissing(employee.Email, "Email");
            //if unsuccessful
            if (!emailResult.Success)
            {
                return emailResult;
            }
            //is the email in the wrong format
            var regex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            if (!regex.IsMatch(employee.Email))
            {
                return new ValidationResult()
                {
                    Success = false,
                    Message = "Employee's Email is in an invalid format."
                };
            }
            //validate phone number length
            var phoneNumberResult = IsValidLength(employee.PhoneNumber.ToString(), 10, "Phone Number");
            if (!phoneNumberResult.Success)
            {
                return new ValidationResult()
                {
                    Success = false,
                    Message = "Employee's Phone Number must be 10 digits"
                };
            }
            //validate addressLine1
            var addressLine1Result = IsItemMissing(employee.AddressLine1, "AddressLine1");
            //if unsuccessful
            if (!addressLine1Result.Success)
            {
                return addressLine1Result;
            }
            //validate city
            var cityResult = IsItemMissing(employee.City, "City");
            //if unsuccessful
            if (!cityResult.Success)
            {
                return cityResult;
            }
            //validate state
            var stateResult = IsItemMissing(employee.State, "State");
            //if unsuccessful
            if (!stateResult.Success)
            {
                return stateResult;
            }
            //validate zip length
            var zipLengthResult = IsValidLength(employee.Zip.ToString(), 5, "Zip");
            if (!zipLengthResult.Success)
            {
                return new ValidationResult()
                {
                    Success = false,
                    Message = "Employee's Zip must be 5 digits"
                };
            }

            //validate dependent's dependent type
            var hasEmptyDependentType = employee.Dependents.Exists(dependent => string.IsNullOrEmpty(dependent.DependentType));
            if (hasEmptyDependentType)
            {
                //return the dependent with an empty dependent type
                var emptyDependentType = employee.Dependents.Find(dependent => string.IsNullOrEmpty(dependent.DependentType));
                //return the validation result
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Dependent {emptyDependentType.DependentId} must have a Dependent Type entered."
                };
            }
            //validate dependent's first names
            var hasEmptyFirstName = employee.Dependents.Exists(dependent => string.IsNullOrEmpty(dependent.FirstName));
            if (hasEmptyFirstName)
            {
                //return the dependent with an empty first name
                var emptyFirstName = employee.Dependents.Find(dependent => string.IsNullOrEmpty(dependent.FirstName));
                //return the validation result
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Dependent {emptyFirstName.DependentId} must have a First Name entered."
                };
            }
            //validate dependent's last names
            var hasEmptyLastName = employee.Dependents.Exists(dependent => string.IsNullOrEmpty(dependent.LastName));
            if (hasEmptyLastName)
            {
                //return the dependent with an empty last name
                var emptyLastName = employee.Dependents.Find(dependent => string.IsNullOrEmpty(dependent.LastName));
                //return the validation result
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Dependent {emptyLastName.FirstName} must have a Last Name entered."
                };
            }
            //validate dependent's email
            var hasEmptyEmail = employee.Dependents.Exists(dependent => dependent.DependentType != "Child" && string.IsNullOrEmpty(dependent.Email));
            if (hasEmptyLastName)
            {
                //return the dependent with an empty email
                var emptyEmail = employee.Dependents.Find(dependent => dependent.DependentType != "Child" && string.IsNullOrEmpty(dependent.Email));
                //return the validation result
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Dependent {emptyEmail.FirstName} must have an email entered."
                };
            }
            //validate dependent's invalid email
            var dependentRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            var hasInvalidEmail = employee.Dependents.Exists(dependent => dependent.DependentType != "Child" && !dependentRegex.IsMatch(dependent.Email));
            if (hasInvalidEmail)
            {
                //return the dependent with an invalid email
                var invalidEmail = employee.Dependents.Find(dependent => dependent.DependentType != "Child" && !dependentRegex.IsMatch(dependent.Email));
                //return the validation result
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Dependent {invalidEmail.FirstName} must have a valid email entered."
                };
            }
            //validate length of phone number
            var hasInvalidPhoneNumber = employee.Dependents.Exists(dependent => dependent.DependentType != "Child" && employee.PhoneNumber.ToString().Length != 10);
            if (hasInvalidPhoneNumber)
            {
                //return the dependent with an empty email
                var invalidPhoneNumber = employee.Dependents.Find(dependent => dependent.DependentType != "Child" && employee.PhoneNumber.ToString().Length != 10);
                //return the validation result
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Dependent {invalidPhoneNumber.FirstName} must have a phone number with 10 digits."
                };
            }
            //validate dependent's addressLine1
            var hasEmptyAddressLine1 = employee.Dependents.Exists(dependent => string.IsNullOrEmpty(dependent.AddressLine1));
            if (hasEmptyAddressLine1)
            {
                //return the dependent with an empty addressline1
                var emptyAddressLine1 = employee.Dependents.Find(dependent => string.IsNullOrEmpty(dependent.AddressLine1));
                //return the validation result
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Dependent {emptyAddressLine1.FirstName} must have an addressLine1 entered."
                };
            }
            //validate dependent's city
            var hasEmptyCity = employee.Dependents.Exists(dependent => string.IsNullOrEmpty(dependent.City));
            if (hasEmptyCity)
            {
                //return the dependent with an empty city
                var emptyCity = employee.Dependents.Find(dependent => string.IsNullOrEmpty(dependent.City));
                //return the validation result
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Dependent {emptyCity.FirstName} must have a city entered."
                };
            }
            //validate dependent's state
            var hasEmptyState = employee.Dependents.Exists(dependent => string.IsNullOrEmpty(dependent.State));
            if (hasEmptyState)
            {
                //return the dependent with an empty state
                var emptyState = employee.Dependents.Find(dependent => string.IsNullOrEmpty(dependent.State));
                //return the validation result
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Dependent {emptyState.FirstName} must have a state entered."
                };
            }
            //validate length of phone number
            var hasInvalidZip = employee.Dependents.Exists(dependent => employee.Zip.ToString().Length != 5);
            if (hasInvalidZip)
            {
                //return the dependent with an empty zip
                var invalidZip = employee.Dependents.Find(dependent => employee.Zip.ToString().Length != 5);
                //return the validation result
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Dependent {invalidZip.FirstName} must have a zip code with 5 digits."
                };
            }

            //return the validation result
            return new ValidationResult()
            {
                Success = true,
                Message = ""
            };
        }
        /*private methods*/
        private ValidationResult IsItemMissing(string item, string itemName)
        {
            //check whether the item is null or empty
            if (string.IsNullOrEmpty(item))
            {
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Employee's {itemName} can't be empty."
                };
            }
            else
            {
                return new ValidationResult()
                {
                    Success = true,
                    Message = ""
                };
            }
        }
        private ValidationResult IsValidLength(string item, int length, string itemName)
        {
            //check whether the item is the correct length
            if (item.Length != length)
            {
                return new ValidationResult()
                {
                    Success = false,
                    Message = $"Employee's {itemName} must be {length} digits."
                };
            }
            else
            {
                return new ValidationResult()
                {
                    Success = true,
                    Message = ""
                };
            }
        }
    }
}

