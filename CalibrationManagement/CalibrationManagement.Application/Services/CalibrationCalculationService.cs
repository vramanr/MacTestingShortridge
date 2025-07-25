using System.Globalization;
using System.Text;

namespace CalibrationManagement.Application.Services
{
    public interface ICalibrationCalculationService
    {
        string Justification(string reading);
        string SetPointDecimal(string reading, string setPoint, string mode, decimal deviation, string type, string calType);
        string PadZero(string reading, string setPoint);
        decimal CalculatePercentageDeviation(decimal actual, decimal expected);
        decimal CalculateAbsoluteDeviation(decimal actual, decimal expected);
        bool IsWithinTolerance(decimal actual, decimal expected, decimal tolerance);
        string DetermineCalibrationStatus(decimal deviation, decimal tolerance);
        decimal CalculateDewPoint(decimal temperature, decimal humidity);
        decimal ConvertFahrenheitToCelsius(decimal fahrenheit);
        decimal ConvertCelsiusToFahrenheit(decimal celsius);
        decimal ConvertPsiToMillibar(decimal psi);
    }

    public class CalibrationCalculationService : ICalibrationCalculationService
    {
        public string Justification(string reading)
        {
            if (string.IsNullOrEmpty(reading))
                return reading ?? string.Empty;

            var trimmedReading = reading.Trim();
            var position = trimmedReading.IndexOf('.');
            var negPosition = trimmedReading.IndexOf('-');
            var length = trimmedReading.Length;
            var decimalPosition = position == -1 ? 0 : length - position;
            var leftDigits = position == -1 ? length : position;

            string newReading;

            if (decimalPosition == 4 && position != -1)
            {
                newReading = trimmedReading.PadLeft(10, ' ');
            }
            else if (decimalPosition == 3 && position != -1)
            {
                newReading = (trimmedReading + " ").PadLeft(10, ' ');
            }
            else if (decimalPosition == 2 && position != -1)
            {
                if (leftDigits > 2)
                {
                    newReading = (trimmedReading + "    ").PadLeft(10, ' ');
                }
                else
                {
                    if (leftDigits == 1 || (negPosition >= 0 && leftDigits == 2))
                    {
                        newReading = (trimmedReading + "  ").PadLeft(10, ' ');
                    }
                    else
                    {
                        newReading = (trimmedReading + "   ").PadLeft(10, ' ');
                    }
                }
            }
            else if (decimalPosition == 1 && position != -1)
            {
                newReading = (trimmedReading + "    ").PadLeft(10, ' ');
            }
            else if (position == -1)
            {
                if (length == 3)
                {
                    newReading = (trimmedReading + "      ").PadLeft(10, ' ');
                }
                else
                {
                    newReading = (trimmedReading + "     ").PadLeft(10, ' ');
                }
            }
            else
            {
                newReading = trimmedReading;
            }

            return newReading;
        }

        public string SetPointDecimal(string reading, string setPoint, string mode, decimal deviation, string type, string calType)
        {
            if (string.IsNullOrEmpty(setPoint))
                return string.Empty;

            var trimmedSetPoint = setPoint.Trim();
            int nDecimal;

            if (!trimmedSetPoint.Contains('.'))
            {
                nDecimal = 0;
            }
            else
            {
                nDecimal = trimmedSetPoint.Length - trimmedSetPoint.IndexOf('.') - 1;
            }

            decimal nDeviation = deviation;

            if (type == "ALLOW")
            {
            }

            var deviationStr = nDeviation.ToString($"F{nDecimal}", CultureInfo.InvariantCulture);
            var newReading = Justification(deviationStr);


            return newReading;
        }

        public string PadZero(string reading, string setPoint)
        {
            if (string.IsNullOrEmpty(reading))
                return string.Empty;

            var cReading = reading.Trim();
            
            if (string.IsNullOrEmpty(setPoint) || !setPoint.Contains('.'))
            {
                return cReading;
            }

            var trimmedSetPoint = setPoint.Trim();
            var nSetPointDecimal = trimmedSetPoint.Length - trimmedSetPoint.IndexOf('.') - 1;
            var nPosition = cReading.IndexOf('.');
            var nLength = cReading.Length;
            var nReadingDecimal = nPosition == -1 ? 0 : cReading.Length - nPosition - 1;

            if (nPosition == 0)
            {
                cReading = "0" + cReading;
                nPosition = 1;
                nLength = cReading.Length;
            }

            if (nPosition == -1 && nSetPointDecimal > 0)
            {
                switch (nSetPointDecimal)
                {
                    case 1:
                        cReading = cReading + ".0";
                        break;
                    case 2:
                        cReading = cReading + ".00";
                        break;
                    case 3:
                        cReading = cReading + ".000";
                        break;
                    case 4:
                        cReading = cReading + ".0000";
                        break;
                }
            }

            if (nLength == nPosition + 1 && nPosition >= 0)
            {
                if (nSetPointDecimal == 0)
                {
                    cReading = cReading.Substring(0, cReading.Length - 1);
                }
                else
                {
                    switch (nSetPointDecimal)
                    {
                        case 1:
                            cReading = cReading + "0";
                            break;
                        case 2:
                            cReading = cReading + "00";
                            break;
                        case 3:
                            cReading = cReading + "000";
                            break;
                        case 4:
                            cReading = cReading + "0000";
                            break;
                    }
                }
            }

            if (nPosition >= 0 && nLength != nPosition + 1)
            {
                nReadingDecimal = cReading.Length - cReading.IndexOf('.') - 1;
                
                if (nSetPointDecimal - nReadingDecimal > 0)
                {
                    var zerosToAdd = nSetPointDecimal - nReadingDecimal;
                    switch (zerosToAdd)
                    {
                        case 1:
                            cReading = cReading + "0";
                            break;
                        case 2:
                            cReading = cReading + "00";
                            break;
                        case 3:
                            cReading = cReading + "000";
                            break;
                        case 4:
                            cReading = cReading + "0000";
                            break;
                    }
                }
                else if (nSetPointDecimal - nReadingDecimal < 0)
                {
                    var decimalIndex = cReading.IndexOf('.');
                    var integerPart = cReading.Substring(0, decimalIndex);
                    var decimalPart = cReading.Substring(decimalIndex + 1);
                    
                    if (decimalPart.Length > nSetPointDecimal)
                    {
                        decimalPart = decimalPart.Substring(0, nSetPointDecimal);
                    }
                    
                    cReading = integerPart + "." + decimalPart;
                }
            }

            return cReading.Trim();
        }

        public decimal CalculatePercentageDeviation(decimal actual, decimal expected)
        {
            if (expected == 0)
                return 0;
            
            return Math.Abs((actual - expected) / expected) * 100;
        }

        public decimal CalculateAbsoluteDeviation(decimal actual, decimal expected)
        {
            return Math.Abs(actual - expected);
        }

        public bool IsWithinTolerance(decimal actual, decimal expected, decimal tolerance)
        {
            var deviation = CalculateAbsoluteDeviation(actual, expected);
            return deviation <= tolerance;
        }

        public string DetermineCalibrationStatus(decimal deviation, decimal tolerance)
        {
            if (deviation <= tolerance)
                return "PASS";
            else if (deviation <= tolerance * 1.5m)
                return "LIMITED";
            else
                return "FAIL";
        }

        public decimal CalculateDewPoint(decimal temperature, decimal humidity)
        {
            var a = 17.27m;
            var b = 237.7m;
            var alpha = ((a * temperature) / (b + temperature)) + (decimal)Math.Log((double)(humidity / 100));
            return (b * alpha) / (a - alpha);
        }

        public decimal ConvertFahrenheitToCelsius(decimal fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }

        public decimal ConvertCelsiusToFahrenheit(decimal celsius)
        {
            return (celsius * 9 / 5) + 32;
        }

        public decimal ConvertPsiToMillibar(decimal psi)
        {
            return psi * 68.9476m;
        }
    }
}
