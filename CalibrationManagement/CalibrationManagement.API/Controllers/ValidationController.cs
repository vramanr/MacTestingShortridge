using Microsoft.AspNetCore.Mvc;
using CalibrationManagement.Application.Services;

namespace CalibrationManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationController : ControllerBase
    {
        private readonly IFormValidationService _validationService;
        private readonly ILogger<ValidationController> _logger;

        public ValidationController(
            IFormValidationService validationService,
            ILogger<ValidationController> logger)
        {
            _validationService = validationService;
            _logger = logger;
        }

        [HttpPost("search-criteria")]
        public async Task<IActionResult> ValidateSearchCriteria([FromBody] SearchCriteriaRequest request)
        {
            try
            {
                var result = await _validationService.ValidateSearchCriteriaAsync(request.OrderNo, request.SerialNo);
                return Ok(new { isValid = result.IsValid, errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating search criteria");
                return StatusCode(500, new { isValid = false, errors = new { general = new[] { "Validation error occurred" } } });
            }
        }

        [HttpPost("order-search")]
        public async Task<IActionResult> ValidateOrderSearch([FromBody] OrderSearchRequest request)
        {
            try
            {
                var result = await _validationService.ValidateOrderSearchAsync(request.CompanyId, request.OrderNo);
                return Ok(new { isValid = result.IsValid, errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating order search");
                return StatusCode(500, new { isValid = false, errors = new { general = new[] { "Validation error occurred" } } });
            }
        }

        [HttpPost("mode-selection")]
        public async Task<IActionResult> ValidateModeSelection([FromBody] ModeSelectionRequest request)
        {
            try
            {
                var result = await _validationService.ValidateModeSelectionAsync(request.CalType, request.SelectedModes, request.CalNo);
                return Ok(new { isValid = result.IsValid, errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating mode selection");
                return StatusCode(500, new { isValid = false, errors = new { general = new[] { "Mode validation error occurred" } } });
            }
        }

        [HttpPost("edit-permissions")]
        public async Task<IActionResult> ValidateEditPermissions([FromBody] EditPermissionsRequest request)
        {
            try
            {
                var result = await _validationService.ValidateEditPermissionsAsync(request.CalNo, request.EditType);
                return Ok(new { isValid = result.IsValid, errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating edit permissions");
                return StatusCode(500, new { isValid = false, errors = new { general = new[] { "Edit permission validation error occurred" } } });
            }
        }

        [HttpPost("calibration-type")]
        public async Task<IActionResult> ValidateCalibrationTypeSelection([FromBody] CalibrationTypeRequest request)
        {
            try
            {
                var result = await _validationService.ValidateCalibrationTypeSelectionAsync(request.CalType);
                return Ok(new { isValid = result.IsValid, errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating calibration type selection");
                return StatusCode(500, new { isValid = false, errors = new { general = new[] { "Calibration type validation error occurred" } } });
            }
        }

        [HttpPost("calibration-entry")]
        public async Task<IActionResult> ValidateCalibrationEntry([FromBody] CalibrationEntryRequest request)
        {
            try
            {
                var result = await _validationService.ValidateCalibrationEntryAsync(request.CalInfo, request.CalData);
                return Ok(new { isValid = result.IsValid, errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating calibration entry");
                return StatusCode(500, new { isValid = false, errors = new { general = new[] { "Calibration entry validation error occurred" } } });
            }
        }
    }

    public class SearchCriteriaRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string SerialNo { get; set; } = string.Empty;
    }

    public class OrderSearchRequest
    {
        public string CompanyId { get; set; } = string.Empty;
        public string OrderNo { get; set; } = string.Empty;
    }

    public class ModeSelectionRequest
    {
        public string CalType { get; set; } = string.Empty;
        public List<string> SelectedModes { get; set; } = new();
        public string CalNo { get; set; } = string.Empty;
    }

    public class EditPermissionsRequest
    {
        public string CalNo { get; set; } = string.Empty;
        public string EditType { get; set; } = string.Empty;
    }

    public class CalibrationTypeRequest
    {
        public string CalType { get; set; } = string.Empty;
    }

    public class CalibrationEntryRequest
    {
        public CreateCalInfoDto CalInfo { get; set; } = new();
        public List<CreateCalDataDto> CalData { get; set; } = new();
    }
}
