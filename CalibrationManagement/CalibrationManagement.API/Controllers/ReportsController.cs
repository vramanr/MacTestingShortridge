using CalibrationManagement.Application.Services;
using CalibrationManagement.Infrastructure.Data;
using CalibrationManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly CalibrationDbContext _context;
        private readonly ICalibrationWorkflowService _calibrationService;

        public ReportsController(CalibrationDbContext context, ICalibrationWorkflowService calibrationService)
        {
            _context = context;
            _calibrationService = calibrationService;
        }

        [HttpGet("calibration-certificate/{calId}")]
        public async Task<ActionResult<CalibrationCertificateReport>> GetCalibrationCertificate(Guid calId)
        {
            var calibration = await _calibrationService.GetCalibrationByIdAsync(calId);
            if (calibration == null)
                return NotFound();

            var calData = await _calibrationService.GetCalibrationDataAsync(calId);
            var standards = await _calibrationService.GetCalibrationStandardsAsync(calId);

            var report = new CalibrationCertificateReport
            {
                Calibration = calibration,
                CalibrationData = calData.ToList(),
                Standards = standards.ToList(),
                GeneratedDate = DateTime.UtcNow
            };

            return Ok(report);
        }

        [HttpGet("calibration-summary")]
        public async Task<ActionResult<CalibrationSummaryReport>> GetCalibrationSummary(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? status,
            [FromQuery] string? techId)
        {
            var query = _context.CalInfo
                .Include(c => c.Company)
                .Where(c => !c.Deleted);

            if (startDate.HasValue)
                query = query.Where(c => c.CalDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.CalDate <= endDate.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.CalStatus == status);

            if (!string.IsNullOrEmpty(techId))
                query = query.Where(c => c.CalTech == techId);

            var calibrations = await query.ToListAsync();

            var report = new CalibrationSummaryReport
            {
                TotalCalibrations = calibrations.Count(),
                CompletedCalibrations = calibrations.Count(c => c.CalStatus == "COMPLETED"),
                PendingCalibrations = calibrations.Count(c => c.CalStatus == "PENDING"),
                InProgressCalibrations = calibrations.Count(c => c.CalStatus == "IN_PROGRESS"),
                Calibrations = calibrations,
                GeneratedDate = DateTime.UtcNow,
                StartDate = startDate,
                EndDate = endDate
            };

            return Ok(report);
        }

        [HttpGet("due-calibrations")]
        public async Task<ActionResult<DueCalibrationReport>> GetDueCalibrations([FromQuery] int daysAhead = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(daysAhead);

            var dueCalibrations = await _context.CalInfo
                .Include(c => c.Company)
                .Where(c => !c.Deleted && 
                           c.DueDate.HasValue && 
                           c.DueDate.Value <= cutoffDate &&
                           c.CalStatus != "COMPLETED")
                .OrderBy(c => c.DueDate)
                .ToListAsync();

            var report = new DueCalibrationReport
            {
                DueCalibrations = dueCalibrations,
                DaysAhead = daysAhead,
                CutoffDate = cutoffDate,
                GeneratedDate = DateTime.UtcNow
            };

            return Ok(report);
        }

        [HttpGet("technician-workload")]
        public async Task<ActionResult<TechnicianWorkloadReport>> GetTechnicianWorkload(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var start = startDate ?? DateTime.UtcNow.AddDays(-30);
            var end = endDate ?? DateTime.UtcNow;

            var workloadData = await _context.CalInfo
                .Where(c => !c.Deleted && 
                           c.CalDate >= start && 
                           c.CalDate <= end)
                .GroupBy(c => c.CalTech)
                .Select(g => new TechnicianWorkload
                {
                    TechnicianId = g.Key,
                    TotalCalibrations = g.Count(),
                    CompletedCalibrations = g.Count(c => c.CalStatus == "COMPLETED"),
                    PendingCalibrations = g.Count(c => c.CalStatus == "PENDING"),
                    InProgressCalibrations = g.Count(c => c.CalStatus == "IN_PROGRESS")
                })
                .ToListAsync();

            var report = new TechnicianWorkloadReport
            {
                WorkloadData = workloadData,
                StartDate = start,
                EndDate = end,
                GeneratedDate = DateTime.UtcNow
            };

            return Ok(report);
        }

        [HttpGet("company-activity")]
        public async Task<ActionResult<CompanyActivityReport>> GetCompanyActivity(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var start = startDate ?? DateTime.UtcNow.AddDays(-90);
            var end = endDate ?? DateTime.UtcNow;

            var activityData = await _context.CalInfo
                .Include(c => c.Company)
                .Where(c => !c.Deleted && 
                           c.CalDate >= start && 
                           c.CalDate <= end)
                .GroupBy(c => new { c.CoId, c.Company!.CoName })
                .Select(g => new CompanyActivity
                {
                    CompanyId = g.Key.CoId,
                    CompanyName = g.Key.CoName,
                    TotalCalibrations = g.Count(),
                    LastCalibrationDate = g.Max(c => c.CalDate)
                })
                .OrderByDescending(ca => ca.TotalCalibrations)
                .ToListAsync();

            var report = new CompanyActivityReport
            {
                ActivityData = activityData,
                StartDate = start,
                EndDate = end,
                GeneratedDate = DateTime.UtcNow
            };

            return Ok(report);
        }
    }

    public class CalibrationCertificateReport
    {
        public CalInfo Calibration { get; set; } = null!;
        public List<CalData> CalibrationData { get; set; } = new();
        public List<CalStandards> Standards { get; set; } = new();
        public DateTime GeneratedDate { get; set; }
    }

    public class CalibrationSummaryReport
    {
        public int TotalCalibrations { get; set; }
        public int CompletedCalibrations { get; set; }
        public int PendingCalibrations { get; set; }
        public int InProgressCalibrations { get; set; }
        public List<CalInfo> Calibrations { get; set; } = new();
        public DateTime GeneratedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class DueCalibrationReport
    {
        public List<CalInfo> DueCalibrations { get; set; } = new();
        public int DaysAhead { get; set; }
        public DateTime CutoffDate { get; set; }
        public DateTime GeneratedDate { get; set; }
    }

    public class TechnicianWorkloadReport
    {
        public List<TechnicianWorkload> WorkloadData { get; set; } = new();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime GeneratedDate { get; set; }
    }

    public class TechnicianWorkload
    {
        public string? TechnicianId { get; set; }
        public int TotalCalibrations { get; set; }
        public int CompletedCalibrations { get; set; }
        public int PendingCalibrations { get; set; }
        public int InProgressCalibrations { get; set; }
    }

    public class CompanyActivityReport
    {
        public List<CompanyActivity> ActivityData { get; set; } = new();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime GeneratedDate { get; set; }
    }

    public class CompanyActivity
    {
        public string? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int TotalCalibrations { get; set; }
        public DateTime? LastCalibrationDate { get; set; }
    }
}
