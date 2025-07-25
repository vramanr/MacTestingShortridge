using CalibrationManagement.Application.Services;
using CalibrationManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StandardsController : ControllerBase
    {
        private readonly ICalibrationWorkflowService _calibrationService;

        public StandardsController(ICalibrationWorkflowService calibrationService)
        {
            _calibrationService = calibrationService;
        }

        [HttpGet("by-calibration/{calId}")]
        public async Task<ActionResult<IEnumerable<CalStandards>>> GetStandardsByCalibration(Guid calId)
        {
            var standards = await _calibrationService.GetCalibrationStandardsAsync(calId);
            return Ok(standards);
        }

        [HttpPost]
        public async Task<ActionResult<CalStandards>> CreateStandard([FromBody] CalStandards standard)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdStandard = await _calibrationService.AddCalibrationStandardAsync(standard);
            return CreatedAtAction(nameof(GetStandardsByCalibration), new { calId = standard.CalId }, createdStandard);
        }
    }
}
