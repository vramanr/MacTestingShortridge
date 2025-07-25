using CalibrationManagement.Core.Entities;
using CalibrationManagement.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Application.Services
{
    public interface IVfpBusinessLogicService
    {
        Task<string> ProcessCalibrationDataAsync(int calNo, string reading, string setPoint, string mode, string type, string calType);
        Task<bool> ValidateCalibrationToleranceAsync(int calNo, decimal deviation);
        Task<string> FormatCalibrationReadingAsync(string reading, string setPoint);
        Task<decimal> CalculateAllowedDeviationAsync(decimal reading, string mode, string calType);
        Task UpdateCalibrationDataFieldAsync(int calNo, string fieldType, string value);
    }

    public class VfpBusinessLogicService : IVfpBusinessLogicService
    {
        private readonly ILogger<VfpBusinessLogicService> _logger;
        private readonly CalibrationDbContext _context;
        private readonly ICalibrationCalculationService _calculationService;

        public VfpBusinessLogicService(
            ILogger<VfpBusinessLogicService> logger,
            CalibrationDbContext context,
            ICalibrationCalculationService calculationService)
        {
            _logger = logger;
            _context = context;
            _calculationService = calculationService;
        }

        public async Task<string> ProcessCalibrationDataAsync(int calNo, string reading, string setPoint, string mode, string type, string calType)
        {
            try
            {
                if (string.IsNullOrEmpty(reading) || string.IsNullOrEmpty(setPoint))
                    return "";

                var paddedReading = _calculationService.PadZero(reading, setPoint);
                
                if (decimal.TryParse(paddedReading, out decimal numericReading))
                {
                    var deviation = type == "ALLOW" 
                        ? await CalculateAllowedDeviationAsync(numericReading, mode, calType)
                        : numericReading;

                    var formattedResult = _calculationService.SetPointDecimal(paddedReading, setPoint, mode, deviation, type, calType);
                    
                    await UpdateCalibrationDataFieldAsync(calNo, type, formattedResult);
                    
                    return formattedResult;
                }

                return paddedReading;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing calibration data for CalNo: {CalNo}, Type: {Type}", calNo, type);
                throw;
            }
        }

        public async Task<bool> ValidateCalibrationToleranceAsync(int calNo, decimal deviation)
        {
            try
            {
                var calData = await _context.CalData
                    .FirstOrDefaultAsync(cd => cd.CalNo == calNo);

                if (calData?.AllowDev == null)
                    return false;

                if (decimal.TryParse(calData.AllowDev, out decimal allowedDeviation))
                {
                    return Math.Abs(deviation) <= Math.Abs(allowedDeviation);
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating calibration tolerance for CalNo: {CalNo}", calNo);
                throw;
            }
        }

        public async Task<string> FormatCalibrationReadingAsync(string reading, string setPoint)
        {
            try
            {
                if (string.IsNullOrEmpty(reading))
                    return "";

                var paddedReading = _calculationService.PadZero(reading, setPoint);
                var justifiedReading = _calculationService.Justification(paddedReading);
                
                return justifiedReading;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error formatting calibration reading: {Reading}", reading);
                throw;
            }
        }

        public async Task<decimal> CalculateAllowedDeviationAsync(decimal reading, string mode, string calType)
        {
            try
            {
                var tolerance = await _context.Tolerances
                    .FirstOrDefaultAsync(t => t.Mode == mode && t.CalType == calType);

                if (tolerance != null)
                {
                    var percentDeviation = reading * (tolerance.Percent ?? 0);
                    var constantDeviation = tolerance.Constant ?? 0;
                    return percentDeviation + constantDeviation;
                }

                return reading * 0.01m + 0.001m;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating allowed deviation for Mode: {Mode}, CalType: {CalType}", mode, calType);
                throw;
            }
        }

        public async Task UpdateCalibrationDataFieldAsync(int calNo, string fieldType, string value)
        {
            try
            {
                var calData = await _context.CalData
                    .FirstOrDefaultAsync(cd => cd.CalNo == calNo);

                if (calData != null)
                {
                    switch (fieldType.ToUpper())
                    {
                        case "ALLOW":
                            calData.AllowDev = value;
                            break;
                        case "ACTUAL":
                            calData.ActualDev = value;
                            break;
                        case "PERCENT":
                            calData.PercntDev = value;
                            break;
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating calibration data field {FieldType} for CalNo: {CalNo}", fieldType, calNo);
                throw;
            }
        }
    }
}
