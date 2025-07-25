using System.Data;
using System.Text;
using CalibrationManagement.Core.Entities;
using CalibrationManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Infrastructure.Data
{
    public class DbfDataImporter
    {
        private readonly CalibrationDbContext _context;
        private readonly string _dbfPath;

        public DbfDataImporter(CalibrationDbContext context, string dbfPath)
        {
            _context = context;
            _dbfPath = dbfPath;
        }

        public async Task ImportAllTablesAsync()
        {
            await ImportCalInfoAsync();
            await ImportCalDataAsync();
            await ImportCalStandardsAsync();
            await ImportCompanyAsync();
            await ImportTolerancesAsync();
            await ImportCalSetupAsync();
            await ImportCalTechsAsync();
            await ImportContactsAsync();
            await ImportOrdersAsync();
            await ImportOrderDetailsAsync();
            await ImportModelNumbersAsync();
            await ImportInvoicesAsync();
            await ImportPaymentDetailsAsync();
            await ImportUsersAsync();
        }

        private async Task ImportCalInfoAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "cal_info.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var calInfo = new CalInfo
                {
                    CalNo = GetStringValue(record, "cal_no"),
                    CoId = GetStringValue(record, "co_id"),
                    SerialNo = GetStringValue(record, "serial_no"),
                    ModelNumber = GetStringValue(record, "model_no"),
                    CalDate = GetDateValue(record, "cal_date"),
                    DueDate = GetDateValue(record, "due_date"),
                    CalType = GetStringValue(record, "cal_type"),
                    TechId = GetStringValue(record, "tech_id"),
                    CalStatus = GetStringValue(record, "cal_status"),
                    Temperature = GetDecimalValue(record, "temperature"),
                    Humidity = GetDecimalValue(record, "humidity"),
                    Pressure = GetDecimalValue(record, "pressure"),
                    Notes = GetStringValue(record, "notes"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.CalInfo.Add(calInfo);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportCalDataAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "cal_data.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var calData = new CalData
                {
                    CalId = GetGuidValue(record, "cal_id"),
                    PointNo = GetIntValue(record, "point_no"),
                    NominalValue = GetDecimalValue(record, "nominal_value"),
                    AsFoundValue = GetDecimalValue(record, "as_found_value"),
                    AsLeftValue = GetDecimalValue(record, "as_left_value"),
                    ToleranceValue = GetDecimalValue(record, "tolerance_value"),
                    Units = GetStringValue(record, "units"),
                    PassFail = GetStringValue(record, "pass_fail"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.CalData.Add(calData);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportCalStandardsAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "cal_standards.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var calStandard = new CalStandards
                {
                    CalId = GetGuidValue(record, "cal_id"),
                    StandardId = GetStringValue(record, "standard_id"),
                    StandardName = GetStringValue(record, "standard_name"),
                    SerialNo = GetStringValue(record, "serial_no"),
                    CalDate = GetDateValue(record, "cal_date"),
                    DueDate = GetDateValue(record, "due_date"),
                    Uncertainty = GetDecimalValue(record, "uncertainty"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.CalStandards.Add(calStandard);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportCompanyAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "company.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var company = new Company
                {
                    CoId = GetStringValue(record, "co_id") ?? string.Empty,
                    CoName = GetStringValue(record, "co_name"),
                    Address1 = GetStringValue(record, "address1"),
                    Address2 = GetStringValue(record, "address2"),
                    City = GetStringValue(record, "city"),
                    State = GetStringValue(record, "state"),
                    ZipCode = GetStringValue(record, "zip_code"),
                    Phone = GetStringValue(record, "phone"),
                    Fax = GetStringValue(record, "fax"),
                    Email = GetStringValue(record, "email"),
                    Website = GetStringValue(record, "website"),
                    TaxRate = GetDecimalValue(record, "tax_rate"),
                    DiscountRate = GetDecimalValue(record, "discount_rate"),
                    Active = GetBoolValue(record, "active"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.Company.Add(company);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportTolerancesAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "tolerances.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var tolerance = new Tolerances
                {
                    CalType = GetStringValue(record, "cal_type"),
                    RangeMin = GetDecimalValue(record, "range_min"),
                    RangeMax = GetDecimalValue(record, "range_max"),
                    ToleranceValue = GetDecimalValue(record, "tolerance_value"),
                    ToleranceType = GetStringValue(record, "tolerance_type"),
                    Units = GetStringValue(record, "units"),
                    Active = GetBoolValue(record, "active"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.Tolerances.Add(tolerance);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportCalSetupAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "cal_setup.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var calSetup = new CalSetup
                {
                    CalType = GetStringValue(record, "cal_type"),
                    SetupName = GetStringValue(record, "setup_name"),
                    SetupDescription = GetStringValue(record, "setup_description"),
                    DefaultPoints = GetIntValue(record, "default_points"),
                    DefaultUnits = GetStringValue(record, "default_units"),
                    Active = GetBoolValue(record, "active"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.CalSetup.Add(calSetup);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportCalTechsAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "cal_techs.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var calTech = new CalTechs
                {
                    TechId = GetStringValue(record, "tech_id") ?? string.Empty,
                    TechName = GetStringValue(record, "tech_name"),
                    TechTitle = GetStringValue(record, "tech_title"),
                    Active = GetBoolValue(record, "active"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.CalTechs.Add(calTech);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportContactsAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "contact.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var contact = new Contact
                {
                    CoId = GetStringValue(record, "co_id"),
                    ContactName = GetStringValue(record, "contact_name"),
                    Title = GetStringValue(record, "title"),
                    Phone = GetStringValue(record, "phone"),
                    Email = GetStringValue(record, "email"),
                    IsPrimary = GetBoolValue(record, "is_primary"),
                    Active = GetBoolValue(record, "active"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.Contacts.Add(contact);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportOrdersAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "ordrstat.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var order = new OrdrStat
                {
                    OrderNo = GetStringValue(record, "order_no") ?? string.Empty,
                    CoId = GetStringValue(record, "co_id"),
                    OrderDate = GetDateValue(record, "order_date"),
                    DueDate = GetDateValue(record, "due_date"),
                    Status = GetStringValue(record, "status"),
                    Priority = GetStringValue(record, "priority"),
                    Notes = GetStringValue(record, "notes"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.OrdrStats.Add(order);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportOrderDetailsAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "ordetail.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var orderDetail = new OrDetail
                {
                    OrderNo = GetStringValue(record, "order_no"),
                    LineNo = GetIntValue(record, "line_no"),
                    SerialNo = GetStringValue(record, "serial_no"),
                    ModelNumber = GetStringValue(record, "model_no"),
                    Description = GetStringValue(record, "description"),
                    Quantity = GetIntValue(record, "quantity"),
                    UnitPrice = GetDecimalValue(record, "unit_price"),
                    TotalPrice = GetDecimalValue(record, "total_price"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.OrderDetails.Add(orderDetail);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportModelNumbersAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "model_no.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var modelNo = new ModelNo
                {
                    SerialNo = GetStringValue(record, "serial_no") ?? string.Empty,
                    ModelNumber = GetStringValue(record, "model_no"),
                    CoId = GetStringValue(record, "co_id"),
                    Description = GetStringValue(record, "description"),
                    CalType = GetStringValue(record, "cal_type"),
                    CalInterval = GetIntValue(record, "cal_interval"),
                    Active = GetBoolValue(record, "active"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.ModelNumbers.Add(modelNo);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportInvoicesAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "invoice.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var invoice = new Invoice
                {
                    InvoiceNo = GetStringValue(record, "invoice_no") ?? string.Empty,
                    CoId = GetStringValue(record, "co_id"),
                    OrderNo = GetStringValue(record, "order_no"),
                    InvDate = GetDateValue(record, "inv_date"),
                    DueDate = GetDateValue(record, "due_date"),
                    Subtotal = GetDecimalValue(record, "subtotal"),
                    TaxAmount = GetDecimalValue(record, "tax_amount"),
                    TotalAmount = GetDecimalValue(record, "total_amount"),
                    PaidAmount = GetDecimalValue(record, "paid_amount"),
                    Balance = GetDecimalValue(record, "balance"),
                    Status = GetStringValue(record, "status"),
                    QaCode = GetStringValue(record, "qa_code"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.Invoices.Add(invoice);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportPaymentDetailsAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "pay_dtl.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var paymentDetail = new PayDtl
                {
                    InvoiceNo = GetStringValue(record, "invoice_no"),
                    PaymentDate = GetDateValue(record, "payment_date"),
                    PaymentAmount = GetDecimalValue(record, "payment_amount"),
                    PaymentMethod = GetStringValue(record, "payment_method"),
                    CheckNo = GetStringValue(record, "check_no"),
                    Notes = GetStringValue(record, "notes"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.PaymentDetails.Add(paymentDetail);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ImportUsersAsync()
        {
            var dbfFile = Path.Combine(_dbfPath, "users.dbf");
            if (!File.Exists(dbfFile)) return;

            var records = ReadDbfFile(dbfFile);
            foreach (var record in records)
            {
                var user = new Users
                {
                    Username = GetStringValue(record, "username") ?? string.Empty,
                    PasswordHash = GetStringValue(record, "password_hash"),
                    FirstName = GetStringValue(record, "first_name"),
                    LastName = GetStringValue(record, "last_name"),
                    Email = GetStringValue(record, "email"),
                    Role = GetStringValue(record, "role"),
                    Active = GetBoolValue(record, "active"),
                    LastLogin = GetDateValue(record, "last_login"),
                    CreatedDate = GetDateValue(record, "created_date") ?? DateTime.UtcNow,
                    Deleted = GetBoolValue(record, "deleted")
                };

                _context.Users.Add(user);
            }

            await _context.SaveChangesAsync();
        }

        private List<Dictionary<string, object>> ReadDbfFile(string filePath)
        {
            var records = new List<Dictionary<string, object>>();
            
            
            
            return records;
        }

        private string? GetStringValue(Dictionary<string, object> record, string fieldName)
        {
            return record.TryGetValue(fieldName, out var value) ? value?.ToString()?.Trim() : null;
        }

        private DateTime? GetDateValue(Dictionary<string, object> record, string fieldName)
        {
            if (record.TryGetValue(fieldName, out var value) && value != null)
            {
                if (DateTime.TryParse(value.ToString(), out var dateValue))
                    return dateValue;
            }
            return null;
        }

        private decimal? GetDecimalValue(Dictionary<string, object> record, string fieldName)
        {
            if (record.TryGetValue(fieldName, out var value) && value != null)
            {
                if (decimal.TryParse(value.ToString(), out var decimalValue))
                    return decimalValue;
            }
            return null;
        }

        private int? GetIntValue(Dictionary<string, object> record, string fieldName)
        {
            if (record.TryGetValue(fieldName, out var value) && value != null)
            {
                if (int.TryParse(value.ToString(), out var intValue))
                    return intValue;
            }
            return null;
        }

        private bool GetBoolValue(Dictionary<string, object> record, string fieldName)
        {
            if (record.TryGetValue(fieldName, out var value) && value != null)
            {
                var stringValue = value.ToString()?.ToLower();
                return stringValue == "true" || stringValue == "t" || stringValue == "1" || stringValue == "yes" || stringValue == "y";
            }
            return false;
        }

        private Guid GetGuidValue(Dictionary<string, object> record, string fieldName)
        {
            if (record.TryGetValue(fieldName, out var value) && value != null)
            {
                if (Guid.TryParse(value.ToString(), out var guidValue))
                    return guidValue;
            }
            return Guid.NewGuid();
        }
    }
}
