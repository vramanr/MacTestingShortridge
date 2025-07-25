using Xunit;
using FluentAssertions;
using CalibrationManagement.Application.Services;

namespace CalibrationManagement.Tests.Services
{
    public class CalibrationCalculationServiceTests
    {
        private readonly CalibrationCalculationService _service;

        public CalibrationCalculationServiceTests()
        {
            _service = new CalibrationCalculationService();
        }

        [Theory]
        [InlineData(100.0, 95.0, 5.0)]
        [InlineData(100.0, 105.0, 5.0)]
        [InlineData(50.0, 52.5, 2.5)]
        [InlineData(25.0, 24.0, 1.0)]
        [InlineData(0.0, 0.0, 0.0)]
        public void CalculateAbsoluteDeviation_ShouldCalculateCorrectly(decimal actual, decimal expected, decimal expectedResult)
        {
            var result = _service.CalculateAbsoluteDeviation(actual, expected);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(100.5, 100.0, 0.5)]
        [InlineData(101.0, 100.0, 1.0)]
        [InlineData(52.5, 50.0, 5.0)]
        [InlineData(100.0, 100.0, 0.0)]
        public void CalculatePercentageDeviation_ShouldCalculateCorrectly(decimal actual, decimal expected, decimal expectedResult)
        {
            var result = _service.CalculatePercentageDeviation(actual, expected);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(100.5, 100.0, 1.0, true)]
        [InlineData(101.5, 100.0, 1.0, false)]
        [InlineData(101.0, 100.0, 1.0, true)]
        [InlineData(99.5, 100.0, 1.0, true)]
        [InlineData(98.5, 100.0, 1.0, false)]
        public void IsWithinTolerance_ShouldValidateCorrectly(decimal actual, decimal expected, decimal tolerance, bool expectedResult)
        {
            var result = _service.IsWithinTolerance(actual, expected, tolerance);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(0.5, 1.0, "PASS")]
        [InlineData(1.5, 1.0, "LIMITED")]
        [InlineData(2.0, 1.0, "FAIL")]
        [InlineData(0.0, 1.0, "PASS")]
        public void DetermineCalibrationStatus_ShouldReturnCorrectStatus(decimal deviation, decimal tolerance, string expected)
        {
            var result = _service.DetermineCalibrationStatus(deviation, tolerance);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(32.0, 0.0)]
        [InlineData(212.0, 100.0)]
        [InlineData(68.0, 20.0)]
        [InlineData(-40.0, -40.0)]
        public void ConvertFahrenheitToCelsius_ShouldConvertCorrectly(decimal fahrenheit, decimal expected)
        {
            var result = _service.ConvertFahrenheitToCelsius(fahrenheit);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0.0, 32.0)]
        [InlineData(100.0, 212.0)]
        [InlineData(20.0, 68.0)]
        [InlineData(-40.0, -40.0)]
        public void ConvertCelsiusToFahrenheit_ShouldConvertCorrectly(decimal celsius, decimal expected)
        {
            var result = _service.ConvertCelsiusToFahrenheit(celsius);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1.0, 68.9476)]
        [InlineData(10.0, 689.476)]
        [InlineData(0.0, 0.0)]
        public void ConvertPsiToMillibar_ShouldConvertCorrectly(decimal psi, decimal expected)
        {
            var result = _service.ConvertPsiToMillibar(psi);

            result.Should().Be(expected);
        }
    }
}
