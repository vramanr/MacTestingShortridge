using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using CalibrationManagement.Application.Services;
using CalibrationManagement.Infrastructure.Data;
using CalibrationManagement.Core.Entities;

namespace CalibrationManagement.Tests.Services
{
    public class CalibrationWorkflowServiceTests : IDisposable
    {
        private readonly CalibrationDbContext _context;
        private readonly Mock<ICalibrationCalculationService> _mockCalculationService;
        private readonly CalibrationWorkflowService _service;

        public CalibrationWorkflowServiceTests()
        {
            var options = new DbContextOptionsBuilder<CalibrationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CalibrationDbContext(options);
            _mockCalculationService = new Mock<ICalibrationCalculationService>();
            _service = new CalibrationWorkflowService(_context, _mockCalculationService.Object);
        }

        [Fact]
        public async Task CreateCalibrationAsync_ShouldGenerateCalNoWhenEmpty()
        {
            var calInfo = new CalInfo
            {
                SerialNo = "TEST123",
                ModelNumber = "MODEL123",
                CalType = "PRESSURE"
            };

            var result = await _service.CreateCalibrationAsync(calInfo);

            result.CalNo.Should().NotBeNullOrEmpty();
            result.CalNo.Should().StartWith(DateTime.Now.Year.ToString());
            result.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task CreateCalibrationAsync_ShouldPreserveExistingCalNo()
        {
            var calInfo = new CalInfo
            {
                CalNo = "EXISTING123",
                SerialNo = "TEST123",
                ModelNumber = "MODEL123",
                CalType = "PRESSURE"
            };

            var result = await _service.CreateCalibrationAsync(calInfo);

            result.CalNo.Should().Be("EXISTING123");
        }

        [Fact]
        public async Task GetCalibrationByIdAsync_ShouldReturnCalibrationWithRelatedData()
        {
            var company = new Company { CoId = "TEST", CoName = "Test Company" };
            var calInfo = new CalInfo
            {
                CalNo = "TEST001",
                SerialNo = "SN123",
                ModelNumber = "MODEL123",
                CalType = "PRESSURE",
                CoId = "TEST",
                Company = company
            };

            _context.Company.Add(company);
            _context.CalInfo.Add(calInfo);
            await _context.SaveChangesAsync();

            var result = await _service.GetCalibrationByIdAsync(calInfo.CalId);

            result.Should().NotBeNull();
            result!.CalNo.Should().Be("TEST001");
            result.Company.Should().NotBeNull();
            result.Company!.CoName.Should().Be("Test Company");
        }

        [Fact]
        public async Task GetCalibrationsByCompanyAsync_ShouldReturnOnlyActiveCalibrations()
        {
            var company = new Company { CoId = "TEST", CoName = "Test Company" };
            var activeCal = new CalInfo
            {
                CalNo = "ACTIVE001",
                CoId = "TEST",
                Deleted = false,
                Company = company
            };
            var deletedCal = new CalInfo
            {
                CalNo = "DELETED001",
                CoId = "TEST",
                Deleted = true,
                Company = company
            };

            _context.Company.Add(company);
            _context.CalInfo.AddRange(activeCal, deletedCal);
            await _context.SaveChangesAsync();

            var result = await _service.GetCalibrationsByCompanyAsync("TEST");

            result.Should().HaveCount(1);
            result.First().CalNo.Should().Be("ACTIVE001");
        }

        [Fact]
        public async Task AddCalibrationDataAsync_ShouldCalculateDeviationAndPassFail()
        {
            _mockCalculationService.Setup(x => x.PadZero(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("100.00");

            var calData = new CalData
            {
                CalId = Guid.NewGuid(),
                SetPoint = 100.0m,
                ActualReading = 99.5m,
                Tolerance = 1.0m,
                Mode = "PRESSURE"
            };

            var result = await _service.AddCalibrationDataAsync(calData);

            result.Deviation.Should().Be(-0.5m);
            result.PercentDeviation.Should().Be(-0.5m);
            result.PassFail.Should().Be("PASS");
        }

        [Fact]
        public async Task AddCalibrationDataAsync_ShouldMarkFailWhenOutOfTolerance()
        {
            _mockCalculationService.Setup(x => x.PadZero(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("100.00");

            var calData = new CalData
            {
                CalId = Guid.NewGuid(),
                SetPoint = 100.0m,
                ActualReading = 102.0m,
                Tolerance = 1.0m,
                Mode = "PRESSURE"
            };

            var result = await _service.AddCalibrationDataAsync(calData);

            result.Deviation.Should().Be(2.0m);
            result.PassFail.Should().Be("FAIL");
        }

        [Fact]
        public async Task ValidateCalibrationDataAsync_ShouldReturnTrueForValidData()
        {
            var calInfo = new CalInfo { CalNo = "TEST001" };
            _context.CalInfo.Add(calInfo);
            await _context.SaveChangesAsync();

            var calData = new CalData
            {
                CalId = calInfo.CalId,
                SetPoint = 100.0m,
                ActualReading = 99.8m,
                Tolerance = 1.0m
            };
            _context.CalData.Add(calData);
            await _context.SaveChangesAsync();

            var result = await _service.ValidateCalibrationDataAsync(calInfo.CalId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateCalibrationDataAsync_ShouldReturnFalseForOutOfToleranceData()
        {
            var calInfo = new CalInfo { CalNo = "TEST001" };
            _context.CalInfo.Add(calInfo);
            await _context.SaveChangesAsync();

            var calData = new CalData
            {
                CalId = calInfo.CalId,
                SetPoint = 100.0m,
                ActualReading = 102.5m,
                Tolerance = 1.0m
            };
            _context.CalData.Add(calData);
            await _context.SaveChangesAsync();

            var result = await _service.ValidateCalibrationDataAsync(calInfo.CalId);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task GenerateCalibrationNumberAsync_ShouldCreateSequentialNumbers()
        {
            var year = DateTime.Now.Year.ToString();
            var existingCal = new CalInfo { CalNo = $"{year}000001" };
            _context.CalInfo.Add(existingCal);
            await _context.SaveChangesAsync();

            var result = await _service.GenerateCalibrationNumberAsync();

            result.Should().Be($"{year}000002");
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
