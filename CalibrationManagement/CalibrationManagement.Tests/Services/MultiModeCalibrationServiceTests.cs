using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using CalibrationManagement.Application.Services;
using CalibrationManagement.Infrastructure.Data;
using CalibrationManagement.Core.Entities;

namespace CalibrationManagement.Tests.Services
{
    public class MultiModeCalibrationServiceTests : IDisposable
    {
        private readonly CalibrationDbContext _context;
        private readonly MultiModeCalibrationService _service;
        private readonly Mock<ILogger<MultiModeCalibrationService>> _logger;

        public MultiModeCalibrationServiceTests()
        {
            var options = new DbContextOptionsBuilder<CalibrationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CalibrationDbContext(options);
            _logger = new Mock<ILogger<MultiModeCalibrationService>>();
            _service = new MultiModeCalibrationService(_logger.Object, _context);

            SeedTestData();
        }

        private void SeedTestData()
        {
            _context.CalSetup.AddRange(
                new CalSetup { CalType = "ADM", Mode = "Temperature", Active = true },
                new CalSetup { CalType = "ADM", Mode = "Humidity", Active = true },
                new CalSetup { CalType = "ADM", Mode = "Pressure", Active = true },
                new CalSetup { CalType = "HDM", Mode = "Humidity", Active = true },
                new CalSetup { CalType = "HDM", Mode = "Dew Point", Active = true }
            );

            _context.SaveChanges();
        }

        [Theory]
        [InlineData("ADM", 3)]
        [InlineData("HDM", 2)]
        [InlineData("INVALID", 0)]
        public async Task GetAvailableModesAsync_ShouldReturnCorrectModes(string calType, int expectedCount)
        {
            var result = await _service.GetAvailableModesAsync(calType);

            result.Should().HaveCount(expectedCount);
        }

        [Theory]
        [InlineData("ADM", new[] { "Temperature", "Humidity" }, true)]
        [InlineData("ADM", new[] { "Temperature", "Humidity", "Pressure" }, true)]
        [InlineData("ADM", new[] { "Flow Eqv", "Velocity Eqv" }, false)]
        [InlineData("HDM", new[] { "Humidity" }, true)]
        public async Task ValidateModeSelectionAsync_ShouldValidateCorrectly(string calType, string[] selectedModes, bool expectedValid)
        {
            var result = await _service.ValidateModeCompatibilityAsync(calType, selectedModes.ToList());

            result.Should().Be(expectedValid);
        }

        [Theory]
        [InlineData("Flow Eqv", "Velocity Eqv", false)]
        [InlineData("Temperature", "Humidity", true)]
        [InlineData("Pressure", "Temperature", true)]
        public async Task AreModesCompatible_ShouldValidateCompatibility(string mode1, string mode2, bool expected)
        {
            var result = await _service.ValidateModeCompatibilityAsync("ADM", new List<string> { mode1, mode2 });

            result.Should().Be(expected);
        }

        [Fact]
        public async Task GetModeConfigurationAsync_ShouldReturnConfiguration()
        {
            var result = await _service.GetModeConfigurationAsync("ADM", "Temperature");

            result.Should().NotBeNull();
            result.CalType.Should().Be("ADM");
            result.Mode.Should().Be("Temperature");
        }

        [Theory]
        [InlineData("ADM", "Temperature", true)]
        [InlineData("ADM", "INVALID", false)]
        [InlineData("INVALID", "Temperature", false)]
        public async Task IsModeValidForCalibrationTypeAsync_ShouldValidateCorrectly(string calType, string mode, bool expected)
        {
            var result = await _service.ValidateModeCompatibilityAsync(calType, new List<string> { mode });

            result.Should().Be(expected);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
