using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Net;
using CalibrationManagement.API;
using CalibrationManagement.Infrastructure.Data;
using CalibrationManagement.Core.Entities;
using CalibrationManagement.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Tests.Controllers
{
    public class CalibrationControllerTests : IClassFixture<TestWebApplicationFactory<CalibrationManagement.API.Program>>
    {
        private readonly TestWebApplicationFactory<CalibrationManagement.API.Program> _factory;
        private readonly HttpClient _client;

        public CalibrationControllerTests(TestWebApplicationFactory<CalibrationManagement.API.Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        private async Task SeedTestDataAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();

            await context.Database.EnsureCreatedAsync();

            context.CalData.RemoveRange(context.CalData);
            context.CalInfo.RemoveRange(context.CalInfo);
            await context.SaveChangesAsync();

            var cal1 = new CalInfo 
            { 
                CalNo = "CAL001", 
                OrderNo = "12345", 
                SerialNo = "SN001", 
                ModelNumber = "MODEL001",
                Status = "Active",
                CalType = "ADM",
                CompanyId = "COMP01",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Deleted = false
            };

            var cal2 = new CalInfo 
            { 
                CalNo = "CAL002", 
                OrderNo = "12346", 
                SerialNo = "SN002", 
                ModelNumber = "MODEL002",
                Status = "Complete",
                CalType = "HDM",
                CompanyId = "COMP02",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Deleted = false
            };

            context.CalInfo.AddRange(cal1, cal2);
            await context.SaveChangesAsync();

            context.CalData.AddRange(
                new CalData 
                { 
                    CalId = cal1.CalId,
                    CalNo = "CAL001", 
                    PointNo = 1, 
                    SetPoint = 100.0m, 
                    Reading = 99.5m, 
                    ActualReading = 99.5m,
                    Deviation = -0.5m,
                    Status = "PASS",
                    CreatedDate = DateTime.UtcNow,
                    Deleted = false
                },
                new CalData 
                { 
                    CalId = cal1.CalId,
                    CalNo = "CAL001", 
                    PointNo = 2, 
                    SetPoint = 200.0m, 
                    Reading = 201.0m, 
                    ActualReading = 201.0m,
                    Deviation = 0.5m,
                    Status = "PASS",
                    CreatedDate = DateTime.UtcNow,
                    Deleted = false
                }
            );

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetCalibrations_ShouldReturnAllCalibrations()
        {
            await SeedTestDataAsync();

            var response = await _client.GetAsync("/api/calibration");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var calibrations = await response.Content.ReadFromJsonAsync<List<CalInfoDto>>();
            calibrations.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetCalibration_WithValidId_ShouldReturnCalibration()
        {
            await SeedTestDataAsync();

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();
            
            var allCalibrations = await context.CalInfo.ToListAsync();
            allCalibrations.Should().HaveCount(2, "because we seeded 2 calibrations");
            
            var calibration = await context.CalInfo.FirstOrDefaultAsync(c => c.CalNo == "CAL001");
            calibration.Should().NotBeNull("because CAL001 should exist in the seeded data");

            var response = await _client.GetAsync($"/api/calibration/{calibration!.CalId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var calibrationDto = await response.Content.ReadFromJsonAsync<CalInfoDto>();
            calibrationDto.Should().NotBeNull();
            calibrationDto!.CalNo.Should().Be("CAL001");
        }

        [Fact]
        public async Task GetCalibration_WithInvalidId_ShouldReturnNotFound()
        {
            var invalidId = Guid.NewGuid();
            var response = await _client.GetAsync($"/api/calibration/{invalidId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateCalibration_WithValidData_ShouldCreateCalibration()
        {
            var newCalibration = new CreateCalInfoDto
            {
                CalNo = "CAL003",
                OrderNo = "12347",
                SerialNo = "SN003",
                ModelNumber = "MODEL003",
                CalType = "ADM",
                CompanyId = "COMP01",
                Status = "Active"
            };

            var response = await _client.PostAsJsonAsync("/api/calibration", newCalibration);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var createdCalibration = await response.Content.ReadFromJsonAsync<CalInfoDto>();
            createdCalibration.Should().NotBeNull();
            createdCalibration!.CalNo.Should().Be("CAL003");
        }

        [Fact]
        public async Task UpdateCalibration_WithValidData_ShouldUpdateCalibration()
        {
            await SeedTestDataAsync();

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();
            var calibration = await context.CalInfo.FirstOrDefaultAsync(c => c.CalNo == "CAL001");
            calibration.Should().NotBeNull();

            var updateCalibration = new UpdateCalInfoDto
            {
                CalId = calibration!.CalId,
                CalNo = "CAL001",
                OrderNo = "12345",
                SerialNo = "SN001-UPDATED",
                CalType = "ADM",
                CompanyId = "COMP01",
                Status = "Updated"
            };

            var response = await _client.PutAsJsonAsync($"/api/calibration/{calibration.CalId}", updateCalibration);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedCalibration = await response.Content.ReadFromJsonAsync<CalInfoDto>();
            updatedCalibration.Should().NotBeNull();
            updatedCalibration!.SerialNo.Should().Be("SN001-UPDATED");
        }

        [Fact]
        public async Task DeleteCalibration_WithValidId_ShouldDeleteCalibration()
        {
            await SeedTestDataAsync();

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();
            var calibration = await context.CalInfo.FirstOrDefaultAsync(c => c.CalNo == "CAL002");
            calibration.Should().NotBeNull();

            var response = await _client.DeleteAsync($"/api/calibration/{calibration!.CalId}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/calibration/{calibration.CalId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SearchCalibrations_WithValidCriteria_ShouldReturnMatchingCalibrations()
        {
            await SeedTestDataAsync();

            var response = await _client.GetAsync("/api/calibration/search?searchTerm=SN001");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var calibrations = await response.Content.ReadFromJsonAsync<List<CalInfoDto>>();
            calibrations.Should().HaveCount(1);
            calibrations![0].SerialNo.Should().Be("SN001");
        }

        [Fact]
        public async Task GetCalibrationData_WithValidCalNo_ShouldReturnCalibrationData()
        {
            await SeedTestDataAsync();

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();
            var calibration = await context.CalInfo.FirstOrDefaultAsync(c => c.CalNo == "CAL001");
            calibration.Should().NotBeNull();

            var response = await _client.GetAsync($"/api/calibration/{calibration!.CalId}/data");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var calData = await response.Content.ReadFromJsonAsync<List<CalDataDto>>();
            calData.Should().HaveCount(2);
            calData![0].CalNo.Should().Be("CAL001");
        }
    }
}
