using CalibrationManagement.Core.Entities;
using CalibrationManagement.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Application.Services
{
    public interface IFormValidationService
    {
        Task<ValidationResult> ValidateCalibrationEntryAsync(CreateCalInfoDto calInfo, List<CreateCalDataDto> calData);
        Task<ValidationResult> ValidateSearchCriteriaAsync(string orderNo, string serialNo);
        Task<ValidationResult> ValidateOrderSearchAsync(string companyId, string orderNo);
        Task<ValidationResult> ValidateModeSelectionAsync(string calType, List<string> selectedModes, string calNo = null);
        Task<ValidationResult> ValidateEditPermissionsAsync(string calNo, string requestedEditType);
        Task<ValidationResult> ValidateCalibrationTypeSelectionAsync(string calType);
        ValidationResult ValidateFlowVelocityConflict(List<string> selectedModes);
        ValidationResult ValidateRequiredFields(object dto);
    }

    public class FormValidationService : IFormValidationService
    {
        private readonly ILogger<FormValidationService> _logger;
        private readonly CalibrationDbContext _context;
        private readonly IMultiModeCalibrationService _multiModeService;

        public FormValidationService(
            ILogger<FormValidationService> logger,
            CalibrationDbContext context,
            IMultiModeCalibrationService multiModeService)
        {
            _logger = logger;
            _context = context;
            _multiModeService = multiModeService;
        }

        public async Task<ValidationResult> ValidateCalibrationEntryAsync(CreateCalInfoDto calInfo, List<CreateCalDataDto> calData)
        {
            var result = new ValidationResult();

            try
            {
                if (string.IsNullOrEmpty(calInfo.SerialNo))
                {
                    result.AddError("SerialNo", "Serial number is required");
                }

                if (string.IsNullOrEmpty(calInfo.ModelNo))
                {
                    result.AddError("ModelNo", "Model number is required");
                }

                if (string.IsNullOrEmpty(calInfo.CalType))
                {
                    result.AddError("CalType", "Calibration type is required");
                }

                if (!string.IsNullOrEmpty(calInfo.CoId))
                {
                    var companyExists = await _context.Companies
                        .AnyAsync(c => c.CoId == calInfo.CoId);
                    
                    if (!companyExists)
                    {
                        result.AddError("CoId", "Company not found");
                    }
                }

                if (!string.IsNullOrEmpty(calInfo.OrderNo))
                {
                    var orderExists = await _context.OrdrStats
                        .AnyAsync(o => o.OrderNo == calInfo.OrderNo);
                    
                    if (!orderExists)
                    {
                        result.AddError("OrderNo", "Order not found");
                    }
                }

                if (calData != null && calData.Any())
                {
                    var modes = calData.Select(cd => cd.Mode).Distinct().ToList();
                    var modeValidation = await ValidateModeSelectionAsync(calInfo.CalType, modes);
                    result.Merge(modeValidation);

                    for (int i = 0; i < calData.Count; i++)
                    {
                        var dataEntry = calData[i];
                        
                        if (dataEntry.SetPoint == 0 && dataEntry.ActualReading == 0)
                        {
                            result.AddError($"CalData[{i}]", "Set point and actual reading cannot both be zero");
                        }

                        if (!string.IsNullOrEmpty(dataEntry.Units) && dataEntry.Units.Length > 20)
                        {
                            result.AddError($"CalData[{i}].Units", "Units cannot exceed 20 characters");
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating calibration entry");
                result.AddError("General", "Validation error occurred");
                return result;
            }
        }

        public async Task<ValidationResult> ValidateSearchCriteriaAsync(string orderNo, string serialNo)
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(orderNo) && string.IsNullOrEmpty(serialNo))
            {
                result.AddError("SearchCriteria", "Search Criteria must be entered! Enter Order # or Serial #.");
                return result;
            }

            if (!string.IsNullOrEmpty(orderNo))
            {
                if (orderNo.Length < 4 || orderNo.Length > 10)
                {
                    result.AddError("OrderNo", "Order number must be 4-10 characters");
                }
            }

            if (!string.IsNullOrEmpty(serialNo))
            {
                if (serialNo.Length < 3 || serialNo.Length > 50)
                {
                    result.AddError("SerialNo", "Serial number must be 3-50 characters");
                }
            }

            return result;
        }

        public async Task<ValidationResult> ValidateOrderSearchAsync(string companyId, string orderNo)
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(companyId) && string.IsNullOrEmpty(orderNo))
            {
                result.AddError("SearchCriteria", "Search Criteria must be entered!");
                return result;
            }

            try
            {
                bool companyFound = false;
                bool orderFound = false;

                if (!string.IsNullOrEmpty(companyId))
                {
                    companyFound = await _context.Companies
                        .AnyAsync(c => c.CoId == companyId.Trim());
                }

                if (!string.IsNullOrEmpty(orderNo))
                {
                    var trimmedOrder = orderNo.Trim();
                    
                    if (!trimmedOrder.StartsWith("R"))
                    {
                        orderFound = await _context.OrdrStats
                            .AnyAsync(o => o.OrderNo == trimmedOrder);
                    }
                    else
                    {
                        orderFound = trimmedOrder.Length >= 2;
                    }
                }

                if (!string.IsNullOrEmpty(companyId) && !companyFound)
                {
                    result.AddError("CompanyId", "Company not found");
                }

                if (!string.IsNullOrEmpty(orderNo) && !orderFound)
                {
                    result.AddError("OrderNo", "Order not found");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating order search for CompanyId: {CompanyId}, OrderNo: {OrderNo}", companyId, orderNo);
                result.AddError("General", "Validation error occurred");
                return result;
            }
        }

        public async Task<ValidationResult> ValidateModeSelectionAsync(string calType, List<string> selectedModes, string calNo = null)
        {
            var result = new ValidationResult();

            try
            {
                var flowVelocityValidation = ValidateFlowVelocityConflict(selectedModes);
                result.Merge(flowVelocityValidation);

                var modeCompatibility = await _multiModeService.ValidateModeCompatibilityAsync(calType, selectedModes);
                if (!modeCompatibility)
                {
                    result.AddError("ModeSelection", "One or more selected modes are not compatible with the calibration type");
                }

                if (!string.IsNullOrEmpty(calNo))
                {
                    var existingCalInfo = await _context.CalInfos
                        .Include(ci => ci.CalData)
                        .FirstOrDefaultAsync(ci => ci.CalNo == calNo);

                    if (existingCalInfo != null)
                    {
                        var existingModes = existingCalInfo.CalData
                            .Where(cd => !string.IsNullOrEmpty(cd.Mode))
                            .Select(cd => cd.Mode)
                            .Distinct()
                            .ToList();

                        var hasExistingFlow = existingModes.Any(m => m == "Flow Eqv");
                        var hasExistingVelocity = existingModes.Any(m => m == "Velocity Eqv");
                        var hasNewFlow = selectedModes.Any(m => m == "Flow Eqv");
                        var hasNewVelocity = selectedModes.Any(m => m == "Velocity Eqv");

                        if ((hasExistingFlow && hasNewVelocity) || (hasExistingVelocity && hasNewFlow))
                        {
                            result.AddError("ModeSelection", "Cannot add Velocity Eqv when Flow Eqv data exists, or vice versa");
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating mode selection for CalType: {CalType}", calType);
                result.AddError("General", "Mode validation error occurred");
                return result;
            }
        }

        public async Task<ValidationResult> ValidateEditPermissionsAsync(string calNo, string requestedEditType)
        {
            var result = new ValidationResult();

            try
            {
                var calInfo = await _context.CalInfos
                    .FirstOrDefaultAsync(ci => ci.CalNo == calNo);

                if (calInfo == null)
                {
                    result.AddError("CalNo", "Calibration record not found");
                    return result;
                }

                if (requestedEditType == "FULL EDIT")
                {
                    if (calInfo.CalStatus == "COMPLETED")
                    {
                        result.AddError("EditPermission", "Cannot perform full edit on completed calibration");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating edit permissions for CalNo: {CalNo}", calNo);
                result.AddError("General", "Edit permission validation error occurred");
                return result;
            }
        }

        public async Task<ValidationResult> ValidateCalibrationTypeSelectionAsync(string calType)
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(calType))
            {
                result.AddError("CalibrationType", "Please select a calibration type.");
                return result;
            }

            var validCalTypes = await _context.CalSetup
                .Where(cs => cs.Active == true)
                .Select(cs => cs.CalType)
                .Distinct()
                .ToListAsync();

            var calTypePrefix = calType.Split(' ')[0]; // Get ADM, HDM, etc.
            if (!validCalTypes.Contains(calTypePrefix))
            {
                result.AddError("CalibrationType", "Invalid calibration type selected");
            }

            return result;
        }

        public ValidationResult ValidateFlowVelocityConflict(List<string> selectedModes)
        {
            var result = new ValidationResult();

            var hasFlowEqv = selectedModes.Any(m => m == "Flow Eqv");
            var hasVelocityEqv = selectedModes.Any(m => m == "Velocity Eqv");

            if (hasFlowEqv && hasVelocityEqv)
            {
                result.AddError("ModeSelection", "Flow Eqv and Velocity Eqv modes cannot be selected simultaneously");
            }

            return result;
        }

        public ValidationResult ValidateRequiredFields(object dto)
        {
            var result = new ValidationResult();

            
            return result;
        }
    }

    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public Dictionary<string, List<string>> Errors { get; } = new();

        public void AddError(string field, string message)
        {
            if (!Errors.ContainsKey(field))
            {
                Errors[field] = new List<string>();
            }
            Errors[field].Add(message);
        }

        public void Merge(ValidationResult other)
        {
            foreach (var error in other.Errors)
            {
                if (!Errors.ContainsKey(error.Key))
                {
                    Errors[error.Key] = new List<string>();
                }
                Errors[error.Key].AddRange(error.Value);
            }
        }

        public List<string> GetAllErrors()
        {
            return Errors.SelectMany(e => e.Value).ToList();
        }
    }
}
