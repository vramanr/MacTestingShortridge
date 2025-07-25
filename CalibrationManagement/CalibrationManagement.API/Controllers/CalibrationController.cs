using CalibrationManagement.Application.Services;
using CalibrationManagement.Application.DTOs;
using CalibrationManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace CalibrationManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalibrationController : ControllerBase
    {
        private readonly ICalibrationWorkflowService _calibrationService;
        private readonly ICalibrationCalculationService _calculationService;
        private readonly IMapper _mapper;

        public CalibrationController(
            ICalibrationWorkflowService calibrationService,
            ICalibrationCalculationService calculationService,
            IMapper mapper)
        {
            _calibrationService = calibrationService;
            _calculationService = calculationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalInfoDto>>> GetCalibrations()
        {
            var calibrations = await _calibrationService.SearchCalibrationsAsync("");
            var calibrationDtos = _mapper.Map<IEnumerable<CalInfoDto>>(calibrations);
            return Ok(calibrationDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalInfoDto>> GetCalibration(Guid id)
        {
            var calibration = await _calibrationService.GetCalibrationByIdAsync(id);
            if (calibration == null)
                return NotFound();

            var calibrationDto = _mapper.Map<CalInfoDto>(calibration);
            return Ok(calibrationDto);
        }

        [HttpGet("by-number/{calNo}")]
        public async Task<ActionResult<CalInfoDto>> GetCalibrationByNumber(string calNo)
        {
            var calibration = await _calibrationService.GetCalibrationByNumberAsync(calNo);
            if (calibration == null)
                return NotFound();

            var calibrationDto = _mapper.Map<CalInfoDto>(calibration);
            return Ok(calibrationDto);
        }

        [HttpGet("by-company/{coId}")]
        public async Task<ActionResult<IEnumerable<CalInfoDto>>> GetCalibrationsByCompany(string coId)
        {
            var calibrations = await _calibrationService.GetCalibrationsByCompanyAsync(coId);
            var calibrationDtos = _mapper.Map<IEnumerable<CalInfoDto>>(calibrations);
            return Ok(calibrationDtos);
        }

        [HttpGet("by-order/{orderNo}")]
        public async Task<ActionResult<IEnumerable<CalInfoDto>>> GetCalibrationsByOrder(string orderNo)
        {
            var calibrations = await _calibrationService.GetCalibrationsByOrderAsync(orderNo);
            var calibrationDtos = _mapper.Map<IEnumerable<CalInfoDto>>(calibrations);
            return Ok(calibrationDtos);
        }

        [HttpGet("by-status/{status}")]
        public async Task<ActionResult<IEnumerable<CalInfoDto>>> GetCalibrationsByStatus(string status)
        {
            var calibrations = await _calibrationService.GetCalibrationsByStatusAsync(status);
            var calibrationDtos = _mapper.Map<IEnumerable<CalInfoDto>>(calibrations);
            return Ok(calibrationDtos);
        }

        [HttpGet("by-technician/{techId}")]
        public async Task<ActionResult<IEnumerable<CalInfoDto>>> GetCalibrationsByTechnician(string techId)
        {
            var calibrations = await _calibrationService.GetCalibrationsByTechnicianAsync(techId);
            var calibrationDtos = _mapper.Map<IEnumerable<CalInfoDto>>(calibrations);
            return Ok(calibrationDtos);
        }

        [HttpGet("by-date-range")]
        public async Task<ActionResult<IEnumerable<CalInfoDto>>> GetCalibrationsByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var calibrations = await _calibrationService.GetCalibrationsByDateRangeAsync(startDate, endDate);
            var calibrationDtos = _mapper.Map<IEnumerable<CalInfoDto>>(calibrations);
            return Ok(calibrationDtos);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CalInfoDto>>> SearchCalibrations([FromQuery] string searchTerm)
        {
            var calibrations = await _calibrationService.SearchCalibrationsAsync(searchTerm ?? "");
            var calibrationDtos = _mapper.Map<IEnumerable<CalInfoDto>>(calibrations);
            return Ok(calibrationDtos);
        }

        [HttpPost]
        public async Task<ActionResult<CalInfoDto>> CreateCalibration([FromBody] CreateCalInfoDto createCalInfoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var calInfo = _mapper.Map<CalInfo>(createCalInfoDto);
            var createdCalibration = await _calibrationService.CreateCalibrationAsync(calInfo);
            var createdCalibrationDto = _mapper.Map<CalInfoDto>(createdCalibration);
            
            return CreatedAtAction(nameof(GetCalibration), new { id = createdCalibration.CalId }, createdCalibrationDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CalInfoDto>> UpdateCalibration(Guid id, [FromBody] UpdateCalInfoDto updateCalInfoDto)
        {
            if (id != updateCalInfoDto.CalId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCalibration = await _calibrationService.GetCalibrationByIdAsync(id);
            if (existingCalibration == null)
                return NotFound();

            var calInfo = _mapper.Map<CalInfo>(updateCalInfoDto);
            var updatedCalibration = await _calibrationService.UpdateCalibrationAsync(calInfo);
            var updatedCalibrationDto = _mapper.Map<CalInfoDto>(updatedCalibration);
            
            return Ok(updatedCalibrationDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCalibration(Guid id)
        {
            var result = await _calibrationService.DeleteCalibrationAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}/data")]
        public async Task<ActionResult<IEnumerable<CalDataDto>>> GetCalibrationData(Guid id)
        {
            var calData = await _calibrationService.GetCalibrationDataAsync(id);
            var calDataDtos = _mapper.Map<IEnumerable<CalDataDto>>(calData);
            return Ok(calDataDtos);
        }

        [HttpPost("{id}/data")]
        public async Task<ActionResult<CalData>> AddCalibrationData(Guid id, [FromBody] CalData calData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            calData.CalId = id;
            var createdData = await _calibrationService.AddCalibrationDataAsync(calData);
            return CreatedAtAction(nameof(GetCalibrationData), new { id }, createdData);
        }

        [HttpPut("data/{dataId}")]
        public async Task<ActionResult<CalData>> UpdateCalibrationData(Guid dataId, [FromBody] CalData calData)
        {
            if (dataId != calData.CalDataId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedData = await _calibrationService.UpdateCalibrationDataAsync(calData);
            return Ok(updatedData);
        }

        [HttpGet("{id}/standards")]
        public async Task<ActionResult<IEnumerable<CalStandards>>> GetCalibrationStandards(Guid id)
        {
            var standards = await _calibrationService.GetCalibrationStandardsAsync(id);
            return Ok(standards);
        }

        [HttpPost("{id}/standards")]
        public async Task<ActionResult<CalStandards>> AddCalibrationStandard(Guid id, [FromBody] CalStandards calStandard)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            calStandard.CalId = id;
            var createdStandard = await _calibrationService.AddCalibrationStandardAsync(calStandard);
            return CreatedAtAction(nameof(GetCalibrationStandards), new { id }, createdStandard);
        }

        [HttpPost("{id}/validate")]
        public async Task<ActionResult<bool>> ValidateCalibration(Guid id)
        {
            var isValid = await _calibrationService.ValidateCalibrationDataAsync(id);
            return Ok(new { IsValid = isValid });
        }

        [HttpPost("generate-number")]
        public async Task<ActionResult<string>> GenerateCalibrationNumber()
        {
            var calNumber = await _calibrationService.GenerateCalibrationNumberAsync();
            return Ok(new { CalibrationNumber = calNumber });
        }

        [HttpPost("calculate/justification")]
        public ActionResult<string> CalculateJustification([FromBody] string reading)
        {
            var result = _calculationService.Justification(reading);
            return Ok(new { FormattedReading = result });
        }

        [HttpPost("calculate/pad-zero")]
        public ActionResult<string> CalculatePadZero([FromBody] PadZeroRequest request)
        {
            var result = _calculationService.PadZero(request.Reading, request.SetPoint);
            return Ok(new { FormattedReading = result });
        }

        [HttpPost("calculate/set-point-decimal")]
        public ActionResult<string> CalculateSetPointDecimal([FromBody] SetPointDecimalRequest request)
        {
            var result = _calculationService.SetPointDecimal(
                request.Reading, 
                request.SetPoint, 
                request.Mode, 
                request.Deviation, 
                request.Type, 
                request.CalType);
            return Ok(new { FormattedReading = result });
        }
    }

    public class PadZeroRequest
    {
        public string Reading { get; set; } = string.Empty;
        public string SetPoint { get; set; } = string.Empty;
    }

    public class SetPointDecimalRequest
    {
        public string Reading { get; set; } = string.Empty;
        public string SetPoint { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public decimal Deviation { get; set; }
        public string Type { get; set; } = string.Empty;
        public string CalType { get; set; } = string.Empty;
    }
}
