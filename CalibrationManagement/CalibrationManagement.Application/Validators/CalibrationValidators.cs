using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CalibrationManagement.Application.Validators
{
    public class SerialNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            var serialNo = value.ToString()!.Trim();
            
            if (serialNo.Length < 3 || serialNo.Length > 50)
            {
                ErrorMessage = $"Serial number must be between 3 and 50 characters";
                return false;
            }

            if (!Regex.IsMatch(serialNo, @"^[A-Z0-9\-_]+$", RegexOptions.IgnoreCase))
            {
                ErrorMessage = "Serial number can only contain letters, numbers, hyphens, and underscores";
                return false;
            }

            return true;
        }
    }

    public class OrderNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            var orderNo = value.ToString()!.Trim();
            
            if (!Regex.IsMatch(orderNo, @"^\d{4,10}$"))
            {
                ErrorMessage = "Order number must be 4-10 digits";
                return false;
            }

            return true;
        }
    }

    public class CalibrationReadingAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (decimal.TryParse(value.ToString(), out decimal reading))
            {
                if (reading < -999999 || reading > 999999)
                {
                    ErrorMessage = "Reading must be between -999999 and 999999";
                    return false;
                }
                return true;
            }

            ErrorMessage = "Invalid reading format";
            return false;
        }
    }

    public class ToleranceValueAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (decimal.TryParse(value.ToString(), out decimal tolerance))
            {
                if (tolerance < 0 || tolerance > 100)
                {
                    ErrorMessage = "Tolerance must be between 0% and 100%";
                    return false;
                }
                return true;
            }

            ErrorMessage = "Tolerance must be a valid number";
            return false;
        }
    }

    public class CompanyCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            var code = value.ToString()!.Trim().ToUpper();
            
            if (code.Length < 2 || code.Length > 10)
            {
                ErrorMessage = "Company code must be between 2 and 10 characters";
                return false;
            }

            if (!Regex.IsMatch(code, @"^[A-Z0-9]+$"))
            {
                ErrorMessage = "Company code can only contain letters and numbers";
                return false;
            }

            return true;
        }
    }

    public class TechnicianIdAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            var techId = value.ToString()!.Trim();
            
            if (techId.Length < 3 || techId.Length > 20)
            {
                ErrorMessage = "Technician ID must be between 3 and 20 characters";
                return false;
            }

            if (!Regex.IsMatch(techId, @"^[A-Z0-9]+$", RegexOptions.IgnoreCase))
            {
                ErrorMessage = "Technician ID can only contain letters and numbers";
                return false;
            }

            return true;
        }
    }

    public class ModelNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            var modelNo = value.ToString()!.Trim();
            
            if (modelNo.Length < 2 || modelNo.Length > 50)
            {
                ErrorMessage = "Model number must be between 2 and 50 characters";
                return false;
            }

            if (!Regex.IsMatch(modelNo, @"^[A-Z0-9\-_\s]+$", RegexOptions.IgnoreCase))
            {
                ErrorMessage = "Model number contains invalid characters";
                return false;
            }

            return true;
        }
    }

    public class CalibrationDueDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (DateTime.TryParse(value.ToString(), out DateTime dueDate))
            {
                var today = DateTime.Today;
                var oneYearFromNow = today.AddYears(1);

                if (dueDate < today)
                {
                    ErrorMessage = "Due date cannot be in the past";
                    return false;
                }

                if (dueDate > oneYearFromNow)
                {
                    ErrorMessage = "Due date cannot be more than one year from now";
                    return false;
                }

                return true;
            }

            ErrorMessage = "Invalid due date format";
            return false;
        }
    }
}
