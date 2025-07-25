using CalibrationManagement.Core.Entities;
using CalibrationManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Application.Services
{
    public interface IToleranceService
    {
        Task<Tolerances> CreateToleranceAsync(Tolerances tolerance);
        Task<Tolerances> UpdateToleranceAsync(Tolerances tolerance);
        Task<Tolerances?> GetToleranceByIdAsync(Guid toleranceId);
        Task<IEnumerable<Tolerances>> GetTolerancesByCalTypeAsync(string calType);
        Task<IEnumerable<Tolerances>> GetTolerancesByRangeAsync(decimal value, string calType);
        Task<decimal?> CalculateToleranceAsync(decimal value, string calType, string mode);
        Task<bool> DeleteToleranceAsync(Guid toleranceId);
        Task<IEnumerable<Tolerances>> GetAllTolerancesAsync();
    }

    public class ToleranceService : IToleranceService
    {
        private readonly CalibrationDbContext _context;

        public ToleranceService(CalibrationDbContext context)
        {
            _context = context;
        }

        public async Task<Tolerances> CreateToleranceAsync(Tolerances tolerance)
        {
            tolerance.CreatedDate = DateTime.UtcNow;

            _context.Tolerances.Add(tolerance);
            await _context.SaveChangesAsync();

            return tolerance;
        }

        public async Task<Tolerances> UpdateToleranceAsync(Tolerances tolerance)
        {
            _context.Tolerances.Update(tolerance);
            await _context.SaveChangesAsync();

            return tolerance;
        }

        public async Task<Tolerances?> GetToleranceByIdAsync(Guid toleranceId)
        {
            return await _context.Tolerances
                .FirstOrDefaultAsync(t => t.ToleranceId == toleranceId && !t.Deleted);
        }

        public async Task<IEnumerable<Tolerances>> GetTolerancesByCalTypeAsync(string calType)
        {
            return await _context.Tolerances
                .Where(t => t.CalType == calType && !t.Deleted && t.Active)
                .OrderBy(t => t.RangeMin)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tolerances>> GetTolerancesByRangeAsync(decimal value, string calType)
        {
            return await _context.Tolerances
                .Where(t => t.CalType == calType && 
                           t.RangeMin <= value && 
                           t.RangeMax >= value && 
                           !t.Deleted && 
                           t.Active)
                .ToListAsync();
        }

        public async Task<decimal?> CalculateToleranceAsync(decimal value, string calType, string mode)
        {
            var tolerances = await _context.Tolerances
                .Where(t => t.CalType == calType && 
                           t.RangeMin <= value && 
                           t.RangeMax >= value && 
                           !t.Deleted && 
                           t.Active)
                .FirstOrDefaultAsync();

            if (tolerances == null)
                return null;


            decimal tolerance = 0;

            if (tolerances.ToleranceType == "PERCENT" && tolerances.PercentTolerance.HasValue)
            {
                tolerance = value * (tolerances.PercentTolerance.Value / 100);
            }
            else if (tolerances.ToleranceType == "ABSOLUTE" && tolerances.ToleranceValue.HasValue)
            {
                tolerance = tolerances.ToleranceValue.Value;
            }
            else if (tolerances.ToleranceType == "COMBINED" && 
                     tolerances.PercentTolerance.HasValue && 
                     tolerances.ConstantTolerance.HasValue)
            {
                tolerance = value * (tolerances.PercentTolerance.Value / 100) + tolerances.ConstantTolerance.Value;
            }

            return tolerance;
        }

        public async Task<bool> DeleteToleranceAsync(Guid toleranceId)
        {
            var tolerance = await _context.Tolerances.FindAsync(toleranceId);
            if (tolerance == null)
                return false;

            tolerance.Deleted = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Tolerances>> GetAllTolerancesAsync()
        {
            return await _context.Tolerances
                .Where(t => !t.Deleted && t.Active)
                .OrderBy(t => t.CalType)
                .ThenBy(t => t.RangeMin)
                .ToListAsync();
        }
    }
}
