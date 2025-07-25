using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using CalibrationManagement.Application.Services;
using CalibrationManagement.Infrastructure.Data;

namespace CalibrationManagement.Tests.Services
{
    public class VfpBusinessLogicServiceTests : IDisposable
    {
        private readonly VfpBusinessLogicService _service;
        private readonly CalibrationDbContext _context;
        private readonly Mock<ILogger<VfpBusinessLogicService>> _logger;

        public VfpBusinessLogicServiceTests()
        {
            var options = new DbContextOptionsBuilder<CalibrationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new CalibrationDbContext(options);
            _logger = new Mock<ILogger<VfpBusinessLogicService>>();
            _service = new VfpBusinessLogicService(_logger.Object, _context);
        }

        [Theory]
        [InlineData("123.456", "123.456")]
        [InlineData("123.46", "123.46")]
        [InlineData("123.5", "123.5")]
        [InlineData("123", "123")]
        [InlineData("0.00", "0.00")]
        [InlineData("-123.46", "-123.46")]
        public void Justification_ShouldFormatNumbersCorrectly(string value, string expected)
        {
            var result = _service.Justification(value);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("123.456", "100.000", "Temperature", 0.456, "ALLOW", "ADM")]
        [InlineData("123.46", "100.00", "Pressure", 0.46, "ALLOW", "ADM")]
        [InlineData("123.5", "100.0", "Humidity", 0.5, "ALLOW", "ADM")]
        public void SetPointDecimal_ShouldFormatSetPointsCorrectly(string reading, string setPoint, string mode, decimal deviation, string type, string calType)
        {
            var result = _service.SetPointDecimal(reading, setPoint, mode, deviation, type, calType);

            result.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("123", "100.000")]
        [InlineData("123", "100.00")]
        [InlineData("123", "100.0")]
        [InlineData("1", "100.0000")]
        public void PadZero_ShouldPadWithZerosCorrectly(string reading, string setPoint)
        {
            var result = _service.PadZero(reading, setPoint);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public void PadZero_WithEmptyReading_ShouldReturnEmpty()
        {
            var result = _service.PadZero("", "100.000");

            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(100.0, 95.0, 5.0, true)]
        [InlineData(100.0, 105.0, 5.0, true)]
        [InlineData(100.0, 94.0, 5.0, false)]
        [InlineData(100.0, 106.0, 5.0, false)]
        [InlineData(0.0, 0.1, 0.2, true)]
        public void IsWithinTolerance_ShouldValidateToleranceCorrectly(decimal setPoint, decimal reading, decimal tolerance, bool expected)
        {
            var result = _service.IsWithinTolerance(setPoint, reading, tolerance);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(100.0, 95.0, -5.0)]
        [InlineData(100.0, 105.0, 5.0)]
        [InlineData(50.0, 52.5, 2.5)]
        [InlineData(0.0, 0.0, 0.0)]
        public void CalculateDeviation_ShouldCalculatePercentageDeviationCorrectly(decimal setPoint, decimal reading, decimal expected)
        {
            var result = _service.CalculateDeviation(setPoint, reading);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("ADM", true)]
        [InlineData("HDM", true)]
        [InlineData("MultiTemp", true)]
        [InlineData("FH", true)]
        [InlineData("INVALID", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidCalibrationType_ShouldValidateCalibrationTypesCorrectly(string calType, bool expected)
        {
            var result = _service.IsValidCalibrationType(calType);

            result.Should().Be(expected);
        }

        [Fact]
        public void GetCalibrationModes_ShouldReturnCorrectModesForADM()
        {
            var result = _service.GetCalibrationModes("ADM");

            result.Should().NotBeEmpty();
            result.Should().Contain("Temperature");
            result.Should().Contain("Humidity");
            result.Should().Contain("Pressure");
        }

        [Fact]
        public void GetCalibrationModes_ShouldReturnCorrectModesForHDM()
        {
            var result = _service.GetCalibrationModes("HDM");

            result.Should().NotBeEmpty();
            result.Should().Contain("Humidity");
            result.Should().Contain("Dew Point");
        }

        [Fact]
        public void ValidateCalibrationTypeAsync_ShouldValidateCalibrationTypes()
        {
            var result = _service.IsValidCalibrationType("ADM");

            result.Should().Be(true);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
