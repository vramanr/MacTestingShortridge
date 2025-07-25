using CalibrationManagement.Infrastructure.Data;
using CalibrationManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechnicianController : ControllerBase
    {
        private readonly CalibrationDbContext _context;

        public TechnicianController(CalibrationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalTechs>>> GetAllTechnicians()
        {
            var technicians = await _context.CalTechs
                .Where(t => !t.Deleted && t.Active)
                .OrderBy(t => t.TechName)
                .ToListAsync();

            return Ok(technicians);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalTechs>> GetTechnician(Guid id)
        {
            var technician = await _context.CalTechs
                .FirstOrDefaultAsync(t => t.CalTechId == id && !t.Deleted);

            if (technician == null)
                return NotFound();

            return Ok(technician);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CalTechs>>> SearchTechnicians([FromQuery] string searchTerm)
        {
            var term = searchTerm?.ToLower() ?? "";
            
            var technicians = await _context.CalTechs
                .Where(t => !t.Deleted && (
                    t.TechName.ToLower().Contains(term) ||
                    t.TechInitials!.ToLower().Contains(term)
                ))
                .OrderBy(t => t.TechName)
                .ToListAsync();

            return Ok(technicians);
        }

        [HttpPost]
        public async Task<ActionResult<CalTechs>> CreateTechnician([FromBody] CalTechs technician)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            technician.CreatedDate = DateTime.UtcNow;
            
            _context.CalTechs.Add(technician);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTechnician), new { id = technician.CalTechId }, technician);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CalTechs>> UpdateTechnician(Guid id, [FromBody] CalTechs technician)
        {
            if (id != technician.CalTechId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingTechnician = await _context.CalTechs.FindAsync(id);
            if (existingTechnician == null)
                return NotFound();

            _context.Entry(existingTechnician).CurrentValues.SetValues(technician);
            await _context.SaveChangesAsync();

            return Ok(technician);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTechnician(Guid id)
        {
            var technician = await _context.CalTechs.FindAsync(id);
            if (technician == null)
                return NotFound();

            technician.Deleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
