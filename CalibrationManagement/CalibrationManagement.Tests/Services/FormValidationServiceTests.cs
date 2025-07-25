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
    public class FormValidationServiceTests : IDisposable
    {
        private readonly CalibrationDbContext _context;
        private readonly FormValidationService _service;
        private readonly Mock<ILogger<FormValidationService>> _logger;
        private readonly Mock<IMultiModeCalibrationService> _multiModeService;

        public FormValidationServiceTests()
        {
            var options = new DbContextOptionsBuilder<CalibrationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CalibrationDbContext(options);
            _logger = new Mock<ILogger<FormValidationService>>();
            _multiModeService = new Mock<IMultiModeCalibrationService>();
            
            _multiModeService.Setup(m => m.ValidateModeCompatibilityAsync("ADM", It.Is<List<string>>(modes => 
                modes.Contains("Temperature") && modes.Contains("Humidity"))))
                .ReturnsAsync(true);
            
            _multiModeService.Setup(m => m.ValidateModeCompatibilityAsync("ADM", It.Is<List<string>>(modes => 
                modes.Contains("Flow Eqv") && modes.Contains("Velocity Eqv"))))
                .ReturnsAsync(false);
            
            _multiModeService.Setup(m => m.ValidateModeCompatibilityAsync("HDM", It.Is<List<string>>(modes => 
                modes.Contains("Humidity"))))
                .ReturnsAsync(true);
            
            _service = new FormValidationService(_logger.Object, _context, _multiModeService.Object);

            SeedTestData();
        }

        private void SeedTestData()
        {
            _context.CalInfo.AddRange(
                new CalInfo { CalNo = "CAL001", OrderNo = "12345", SerialNo = "SN001", Status = "Active" },
                new CalInfo { CalNo = "CAL002", OrderNo = "12346", SerialNo = "SN002", Status = "Complete" }
            );

            _context.Company.AddRange(
                new Company { CoId = "COMP01", CoName = "Test Company 1" },
                new Company { CoId = "COMP02", CoName = "Test Company 2" }
            );

            _context.SaveChanges();
        }

        [Theory]
        [InlineData("", "", false)]
        [InlineData("12345", "", true)]
        [InlineData("", "SN001", true)]
        [InlineData("12345", "SN001", true)]
        public async Task ValidateSearchCriteriaAsync_ShouldValidateSearchCriteria(string orderNo, string serialNo, bool expectedValid)
        {
            var result = await _service.ValidateSearchCriteriaAsync(orderNo, serialNo);

            result.IsValid.Should().Be(expectedValid);
            if (!expectedValid)
            {
                result.Errors.Should().ContainKey("searchCriteria");
                result.Errors["searchCriteria"].Should().Contain("Search Criteria must be entered! Enter Order # or Serial #.");
            }
        }

        [Theory]
        [InlineData("COMP01", "12345", true)]
        [InlineData("INVALID", "12345", false)]
        [InlineData("", "", false)]
        public async Task ValidateOrderSearchAsync_ShouldValidateOrderSearch(string companyId, string orderNo, bool expectedValid)
        {
            var result = await _service.ValidateOrderSearchAsync(companyId, orderNo);

            result.IsValid.Should().Be(expectedValid);
        }

        [Theory]
        [InlineData("ADM", new[] { "Temperature", "Humidity" }, true)]
        [InlineData("ADM", new[] { "Flow Eqv", "Velocity Eqv" }, false)]
        [InlineData("HDM", new[] { "Humidity" }, true)]
        public async Task ValidateModeSelectionAsync_ShouldValidateModeSelection(string calType, string[] selectedModes, bool expectedValid)
        {
            var result = await _service.ValidateModeSelectionAsync(calType, selectedModes.ToList(), "CAL001");

            result.IsValid.Should().Be(expectedValid);
        }

        [Theory]
        [InlineData("CAL001", "EDIT", true)]
        [InlineData("CAL002", "EDIT", false)]
        [InlineData("INVALID", "EDIT", false)]
        public async Task ValidateEditPermissionsAsync_ShouldValidateEditPermissions(string calNo, string editType, bool expectedValid)
        {
            var result = await _service.ValidateEditPermissionsAsync(calNo, editType);

            result.IsValid.Should().Be(expectedValid);
        }

        [Theory]
        [InlineData("ADM", true)]
        [InlineData("HDM", true)]
        [InlineData("INVALID", false)]
        [InlineData("", false)]
        public async Task ValidateCalibrationTypeSelectionAsync_ShouldValidateCalibrationTypeSelection(string calType, bool expectedValid)
        {
            var result = await _service.ValidateCalibrationTypeSelectionAsync(calType);

            result.IsValid.Should().Be(expectedValid);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
