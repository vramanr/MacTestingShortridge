using CalibrationManagement.Core.Entities;
using CalibrationManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Application.Services
{
    public interface ICalibrationWorkflowService
    {
        Task<CalInfo> CreateCalibrationAsync(CalInfo calInfo);
        Task<CalInfo> UpdateCalibrationAsync(CalInfo calInfo);
        Task<CalInfo?> GetCalibrationByIdAsync(Guid calId);
        Task<CalInfo?> GetCalibrationByNumberAsync(string calNo);
        Task<IEnumerable<CalInfo>> GetCalibrationsByCompanyAsync(string coId);
        Task<IEnumerable<CalInfo>> GetCalibrationsByOrderAsync(string orderNo);
        Task<IEnumerable<CalInfo>> GetCalibrationsByStatusAsync(string status);
        Task<IEnumerable<CalInfo>> GetCalibrationsByTechnicianAsync(string techId);
        Task<IEnumerable<CalInfo>> GetCalibrationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<CalInfo>> SearchCalibrationsAsync(string searchTerm);
        Task<bool> DeleteCalibrationAsync(Guid calId);
        Task<CalData> AddCalibrationDataAsync(CalData calData);
        Task<CalData> UpdateCalibrationDataAsync(CalData calData);
        Task<IEnumerable<CalData>> GetCalibrationDataAsync(Guid calId);
        Task<CalStandards> AddCalibrationStandardAsync(CalStandards calStandard);
        Task<IEnumerable<CalStandards>> GetCalibrationStandardsAsync(Guid calId);
        Task<bool> ValidateCalibrationDataAsync(Guid calId);
        Task<string> GenerateCalibrationNumberAsync();
    }

    public class CalibrationWorkflowService : ICalibrationWorkflowService
    {
        private readonly CalibrationDbContext _context;
        private readonly ICalibrationCalculationService _calculationService;

        public CalibrationWorkflowService(CalibrationDbContext context, ICalibrationCalculationService calculationService)
        {
            _context = context;
            _calculationService = calculationService;
        }

        public async Task<CalInfo> CreateCalibrationAsync(CalInfo calInfo)
        {
            if (string.IsNullOrEmpty(calInfo.CalNo))
            {
                calInfo.CalNo = await GenerateCalibrationNumberAsync();
            }

            calInfo.CreatedDate = DateTime.UtcNow;
            calInfo.ModifiedDate = DateTime.UtcNow;

            _context.CalInfos.Add(calInfo);
            await _context.SaveChangesAsync();

            return calInfo;
        }

        public async Task<CalInfo> UpdateCalibrationAsync(CalInfo calInfo)
        {
            calInfo.ModifiedDate = DateTime.UtcNow;
            
            _context.CalInfos.Update(calInfo);
            await _context.SaveChangesAsync();

            return calInfo;
        }

        public async Task<CalInfo?> GetCalibrationByIdAsync(Guid calId)
        {
            return await _context.CalInfos
                .Include(c => c.Company)
                .Include(c => c.CalData)
                .Include(c => c.CalStandards)
                .FirstOrDefaultAsync(c => c.CalId == calId && !c.Deleted);
        }

        public async Task<CalInfo?> GetCalibrationByNumberAsync(string calNo)
        {
            return await _context.CalInfos
                .Include(c => c.Company)
                .Include(c => c.CalData)
                .Include(c => c.CalStandards)
                .FirstOrDefaultAsync(c => c.CalNo == calNo && !c.Deleted);
        }

        public async Task<IEnumerable<CalInfo>> GetCalibrationsByCompanyAsync(string coId)
        {
            return await _context.CalInfos
                .Include(c => c.Company)
                .Where(c => c.CoId == coId && !c.Deleted)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CalInfo>> GetCalibrationsByOrderAsync(string orderNo)
        {
            return await _context.CalInfos
                .Include(c => c.Company)
                .Where(c => c.OrderNo == orderNo && !c.Deleted)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CalInfo>> GetCalibrationsByStatusAsync(string status)
        {
            return await _context.CalInfos
                .Include(c => c.Company)
                .Where(c => c.CalStatus == status && !c.Deleted)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CalInfo>> GetCalibrationsByTechnicianAsync(string techId)
        {
            return await _context.CalInfos
                .Include(c => c.Company)
                .Where(c => c.CalTech == techId && !c.Deleted)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CalInfo>> GetCalibrationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.CalInfos
                .Include(c => c.Company)
                .Where(c => c.CalDate >= startDate && c.CalDate <= endDate && !c.Deleted)
                .OrderByDescending(c => c.CalDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CalInfo>> SearchCalibrationsAsync(string searchTerm)
        {
            var term = searchTerm.ToLower();
            
            return await _context.CalInfos
                .Include(c => c.Company)
                .Where(c => !c.Deleted && (
                    c.CalNo.ToLower().Contains(term) ||
                    c.SerialNo!.ToLower().Contains(term) ||
                    c.ModelNo!.ToLower().Contains(term) ||
                    c.Company!.CoName!.ToLower().Contains(term)
                ))
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> DeleteCalibrationAsync(Guid calId)
        {
            var calibration = await _context.CalInfos.FindAsync(calId);
            if (calibration == null)
                return false;

            calibration.Deleted = true;
            calibration.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CalData> AddCalibrationDataAsync(CalData calData)
        {
            calData.CreatedDate = DateTime.UtcNow;
            
            if (calData.SetPoint.HasValue && calData.ActualReading.HasValue)
            {
                var deviation = calData.ActualReading.Value - calData.SetPoint.Value;
                calData.Deviation = deviation;

                if (calData.SetPoint.Value != 0)
                {
                    calData.PercentDeviation = (deviation / calData.SetPoint.Value) * 100;
                }

                var setPointStr = calData.SetPoint.Value.ToString();
                var actualStr = calData.ActualReading.Value.ToString();
                var formattedActual = _calculationService.PadZero(actualStr, setPointStr);
                
                if (calData.Tolerance.HasValue)
                {
                    calData.PassFail = Math.Abs(deviation) <= calData.Tolerance.Value ? "PASS" : "FAIL";
                }
            }

            _context.CalData.Add(calData);
            await _context.SaveChangesAsync();

            return calData;
        }

        public async Task<CalData> UpdateCalibrationDataAsync(CalData calData)
        {
            _context.CalData.Update(calData);
            await _context.SaveChangesAsync();

            return calData;
        }

        public async Task<IEnumerable<CalData>> GetCalibrationDataAsync(Guid calId)
        {
            return await _context.CalData
                .Where(cd => cd.CalId == calId && !cd.Deleted)
                .OrderBy(cd => cd.SequenceNo)
                .ToListAsync();
        }

        public async Task<CalStandards> AddCalibrationStandardAsync(CalStandards calStandard)
        {
            calStandard.CreatedDate = DateTime.UtcNow;
            
            _context.CalStandards.Add(calStandard);
            await _context.SaveChangesAsync();

            return calStandard;
        }

        public async Task<IEnumerable<CalStandards>> GetCalibrationStandardsAsync(Guid calId)
        {
            return await _context.CalStandards
                .Where(cs => cs.CalId == calId && !cs.Deleted)
                .ToListAsync();
        }

        public async Task<bool> ValidateCalibrationDataAsync(Guid calId)
        {
            var calData = await GetCalibrationDataAsync(calId);
            
            foreach (var data in calData)
            {
                if (!data.SetPoint.HasValue || !data.ActualReading.HasValue)
                    return false;

                if (!data.Tolerance.HasValue)
                    return false;

                var deviation = Math.Abs(data.ActualReading.Value - data.SetPoint.Value);
                if (deviation > data.Tolerance.Value)
                    return false;
            }

            return true;
        }

        public async Task<string> GenerateCalibrationNumberAsync()
        {
            var year = DateTime.Now.Year.ToString();
            var lastCal = await _context.CalInfos
                .Where(c => c.CalNo.StartsWith(year))
                .OrderByDescending(c => c.CalNo)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastCal != null && lastCal.CalNo.Length > 4)
            {
                var numberPart = lastCal.CalNo.Substring(4);
                if (int.TryParse(numberPart, out var lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{year}{nextNumber:D6}";
        }
    }
}
