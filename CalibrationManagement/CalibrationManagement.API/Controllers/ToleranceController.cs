using CalibrationManagement.Application.Services;
using CalibrationManagement.Application.DTOs;
using CalibrationManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace CalibrationManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToleranceController : ControllerBase
    {
        private readonly IToleranceService _toleranceService;
        private readonly IMapper _mapper;

        public ToleranceController(IToleranceService toleranceService, IMapper mapper)
        {
            _toleranceService = toleranceService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToleranceDto>>> GetAllTolerances()
        {
            var tolerances = await _toleranceService.GetAllTolerancesAsync();
            var toleranceDtos = _mapper.Map<IEnumerable<ToleranceDto>>(tolerances);
            return Ok(toleranceDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToleranceDto>> GetTolerance(Guid id)
        {
            var tolerance = await _toleranceService.GetToleranceByIdAsync(id);
            if (tolerance == null)
                return NotFound();

            var toleranceDto = _mapper.Map<ToleranceDto>(tolerance);
            return Ok(toleranceDto);
        }

        [HttpGet("by-cal-type/{calType}")]
        public async Task<ActionResult<IEnumerable<ToleranceDto>>> GetTolerancesByCalType(string calType)
        {
            var tolerances = await _toleranceService.GetTolerancesByCalTypeAsync(calType);
            var toleranceDtos = _mapper.Map<IEnumerable<ToleranceDto>>(tolerances);
            return Ok(toleranceDtos);
        }

        [HttpGet("by-range")]
        public async Task<ActionResult<IEnumerable<ToleranceDto>>> GetTolerancesByRange(
            [FromQuery] decimal value, 
            [FromQuery] string calType)
        {
            var tolerances = await _toleranceService.GetTolerancesByRangeAsync(value, calType);
            var toleranceDtos = _mapper.Map<IEnumerable<ToleranceDto>>(tolerances);
            return Ok(toleranceDtos);
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<decimal?>> CalculateTolerance([FromBody] ToleranceCalculationRequest request)
        {
            var tolerance = await _toleranceService.CalculateToleranceAsync(request.Value, request.CalType, request.Mode);
            return Ok(new { Tolerance = tolerance });
        }

        [HttpPost]
        public async Task<ActionResult<ToleranceDto>> CreateTolerance([FromBody] CreateToleranceDto createToleranceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tolerance = _mapper.Map<Tolerances>(createToleranceDto);
            var createdTolerance = await _toleranceService.CreateToleranceAsync(tolerance);
            var createdToleranceDto = _mapper.Map<ToleranceDto>(createdTolerance);
            
            return CreatedAtAction(nameof(GetTolerance), new { id = createdTolerance.ToleranceId }, createdToleranceDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToleranceDto>> UpdateTolerance(Guid id, [FromBody] UpdateToleranceDto updateToleranceDto)
        {
            if (id != updateToleranceDto.ToleranceId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingTolerance = await _toleranceService.GetToleranceByIdAsync(id);
            if (existingTolerance == null)
                return NotFound();

            var tolerance = _mapper.Map<Tolerances>(updateToleranceDto);
            var updatedTolerance = await _toleranceService.UpdateToleranceAsync(tolerance);
            var updatedToleranceDto = _mapper.Map<ToleranceDto>(updatedTolerance);
            
            return Ok(updatedToleranceDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTolerance(Guid id)
        {
            var result = await _toleranceService.DeleteToleranceAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    public class ToleranceCalculationRequest
    {
        public decimal Value { get; set; }
        public string CalType { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
    }
}
