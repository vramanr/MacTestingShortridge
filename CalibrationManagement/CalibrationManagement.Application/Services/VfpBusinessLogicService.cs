using CalibrationManagement.Core.Entities;
using CalibrationManagement.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Application.Services
{
    public interface IVfpBusinessLogicService
    {
        string Justification(string cReading);
        string Justification(string cReading, string cSetPoint);
        string SetPointDecimal(string cReading, string cSetPoint, string cMode, decimal nDeviation, string cType, string cCalType, string cMode2);
        string PadZero(string cReading, string cSetPoint);
        decimal CalculateDeviation(decimal setPoint, decimal reading);
        decimal CalculatePercentDeviation(decimal deviation, decimal setPoint);
        bool IsWithinTolerance(decimal deviation, decimal tolerance);
        bool IsWithinTolerance(decimal actual, decimal expected, decimal tolerance);
        bool IsValidCalibrationType(string calType);
        List<string> GetCalibrationModes(string calType);
        Task<decimal> GetToleranceAsync(string mode, string calType);
    }

    public class VfpBusinessLogicService : IVfpBusinessLogicService
    {
        private readonly ILogger<VfpBusinessLogicService> _logger;
        private readonly CalibrationDbContext _context;

        public VfpBusinessLogicService(
            ILogger<VfpBusinessLogicService> logger,
            CalibrationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public string Justification(string cReading)
        {
            if (string.IsNullOrEmpty(cReading))
                return string.Empty;

            return cReading.Trim();
        }

        public string SetPointDecimal(string cReading, string cSetPoint, string cMode, decimal nDeviation, string cType, string cCalType)
        {
            if (string.IsNullOrEmpty(cSetPoint))
                return string.Empty;

            var trimmedSetPoint = cSetPoint.Trim();
            int nDecimal;

            if (trimmedSetPoint.IndexOf('.') == -1)
            {
                nDecimal = 0;
            }
            else
            {
                nDecimal = trimmedSetPoint.Length - trimmedSetPoint.IndexOf('.') - 1;
            }

            decimal calculatedDeviation = nDeviation;

            if (cType == "ALLOW")
            {
                
                if (decimal.TryParse(cReading, out decimal readingValue))
                {
                    decimal percent = 0.001m; // atolerances[1,1] - percentage tolerance
                    decimal constant = 0.0001m; // atolerances[1,2] - constant tolerance
                    calculatedDeviation = readingValue * percent + constant;
                }
            }

            var deviationStr = calculatedDeviation.ToString($"F{nDecimal}");
            var formattedDeviation = Justification(deviationStr.Trim());

            return formattedDeviation;
        }

        public string PadZero(string cReading, string cSetPoint)
        {
            if (string.IsNullOrEmpty(cReading))
                return string.Empty;

            if (string.IsNullOrEmpty(cSetPoint))
                return cReading.Trim();

            var reading = cReading.Trim();
            var setPoint = cSetPoint.Trim();

            if (setPoint.IndexOf('.') == -1)
            {
                return reading;
            }

            var nSetPointDecimal = setPoint.Length - setPoint.IndexOf('.') - 1;
            var nPosition = reading.IndexOf('.');
            var nLength = reading.Length;
            var nReadingDecimal = nPosition == -1 ? 0 : reading.Length - nPosition - 1;

            if (nPosition == 0)
            {
                reading = "0" + reading;
                nPosition = 1;
                nLength++;
                nReadingDecimal = reading.Length - nPosition - 1;
            }

            if (nPosition == -1 && nSetPointDecimal > 0)
            {
                switch (nSetPointDecimal)
                {
                    case 1:
                        reading = reading + ".0";
                        break;
                    case 2:
                        reading = reading + ".00";
                        break;
                    case 3:
                        reading = reading + ".000";
                        break;
                    case 4:
                        reading = reading + ".0000";
                        break;
                }
                return reading;
            }

            if (nLength == nPosition + 1 && nPosition > 0)
            {
                if (nSetPointDecimal == 0)
                {
                    reading = reading.Substring(0, reading.Length - 1);
                }
                else
                {
                    switch (nSetPointDecimal)
                    {
                        case 1:
                            reading = reading + "0";
                            break;
                        case 2:
                            reading = reading + "00";
                            break;
                        case 3:
                            reading = reading + "000";
                            break;
                        case 4:
                            reading = reading + "0000";
                            break;
                    }
                }
                return reading;
            }

            if (nPosition > 0 && nLength != nPosition + 1)
            {
                var decimalDiff = nSetPointDecimal - nReadingDecimal;
                
                if (decimalDiff > 0)
                {
                    switch (decimalDiff)
                    {
                        case 1:
                            reading = reading + "0";
                            break;
                        case 2:
                            reading = reading + "00";
                            break;
                        case 3:
                            reading = reading + "000";
                            break;
                        case 4:
                            reading = reading + "0000";
                            break;
                    }
                }
                else if (decimalDiff < 0)
                {
                    var integerPart = reading.Substring(0, nPosition);
                    var decimalPart = reading.Substring(nPosition + 1);
                    if (decimalPart.Length > nSetPointDecimal)
                    {
                        decimalPart = decimalPart.Substring(0, nSetPointDecimal);
                    }
                    reading = integerPart + "." + decimalPart;
                }
            }

            return reading;
        }

        public decimal CalculateDeviation(decimal setPoint, decimal reading)
        {
            return reading - setPoint;
        }

        public decimal CalculatePercentDeviation(decimal deviation, decimal setPoint)
        {
            if (setPoint == 0)
                return 0;

            return (deviation / setPoint) * 100;
        }

        public bool IsWithinTolerance(decimal deviation, decimal tolerance)
        {
            return Math.Abs(deviation) <= tolerance;
        }

        public async Task<decimal> GetToleranceAsync(string mode, string calType)
        {
            try
            {
                var tolerance = await _context.Tolerances
                    .FirstOrDefaultAsync(t => t.Mode == mode && t.CalType == calType);

                if (tolerance != null)
                {
                    return (tolerance.Percent ?? 0) + (tolerance.Constant ?? 0);
                }

                return 0.001m;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tolerance for Mode: {Mode}, CalType: {CalType}", mode, calType);
                return 0.001m;
            }
        }

        public string Justification(string cReading, string cSetPoint)
        {
            return Justification(cReading);
        }

        public string SetPointDecimal(string cReading, string cSetPoint, string cMode, decimal nDeviation, string cType, string cCalType, string cMode2)
        {
            return SetPointDecimal(cReading, cSetPoint, cMode, nDeviation, cType, cCalType);
        }

        public bool IsWithinTolerance(decimal actual, decimal expected, decimal tolerance)
        {
            var deviation = Math.Abs(actual - expected);
            return deviation <= tolerance;
        }

        public bool IsValidCalibrationType(string calType)
        {
            if (string.IsNullOrEmpty(calType))
                return false;

            var validTypes = new[] { "ADM", "HDM", "MultiTemp", "FH" };
            return validTypes.Contains(calType);
        }

        public List<string> GetCalibrationModes(string calType)
        {
            if (string.IsNullOrEmpty(calType))
                return new List<string>();

            return calType switch
            {
                "ADM" => new List<string> { "Temperature", "Humidity", "Pressure" },
                "HDM" => new List<string> { "Humidity", "Dew Point" },
                "MultiTemp" => new List<string> { "Temperature", "Multi-Point" },
                "FH" => new List<string> { "Flow", "Humidity" },
                _ => new List<string>()
            };
        }
    }
}
