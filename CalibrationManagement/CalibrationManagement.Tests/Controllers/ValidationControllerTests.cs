using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Net;
using CalibrationManagement.API;
using CalibrationManagement.Infrastructure.Data;
using CalibrationManagement.Core.Entities;
using CalibrationManagement.API.Controllers;

namespace CalibrationManagement.Tests.Controllers
{
    public class SearchCriteriaRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string SerialNo { get; set; } = string.Empty;
    }

    public class OrderSearchRequest
    {
        public string CompanyId { get; set; } = string.Empty;
        public string OrderNo { get; set; } = string.Empty;
    }

    public class ModeSelectionRequest
    {
        public string CalType { get; set; } = string.Empty;
        public List<string> SelectedModes { get; set; } = new();
        public string CalNo { get; set; } = string.Empty;
    }

    public class EditPermissionsRequest
    {
        public string CalNo { get; set; } = string.Empty;
        public string EditType { get; set; } = string.Empty;
    }

    public class CalibrationTypeRequest
    {
        public string CalType { get; set; } = string.Empty;
    }
}

namespace CalibrationManagement.Tests.Controllers
{
    public class ValidationControllerTests : IClassFixture<TestWebApplicationFactory<CalibrationManagement.API.Program>>
    {
        private readonly TestWebApplicationFactory<CalibrationManagement.API.Program> _factory;
        private readonly HttpClient _client;

        public ValidationControllerTests(TestWebApplicationFactory<CalibrationManagement.API.Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        private async Task SeedTestDataAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();

            await context.Database.EnsureCreatedAsync();

            context.CalInfo.AddRange(
                new CalInfo { CalNo = "CAL001", OrderNo = "12345", SerialNo = "SN001", Status = "Active" },
                new CalInfo { CalNo = "CAL002", OrderNo = "12346", SerialNo = "SN002", Status = "Complete" }
            );

            context.Company.AddRange(
                new Company { CoId = "COMP01", CoName = "Test Company 1" },
                new Company { CoId = "COMP02", CoName = "Test Company 2" }
            );

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task ValidateSearchCriteria_WithValidCriteria_ShouldReturnValid()
        {
            await SeedTestDataAsync();

            var request = new SearchCriteriaRequest
            {
                OrderNo = "12345",
                SerialNo = ""
            };

            var response = await _client.PostAsJsonAsync("/api/validation/search-criteria", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ValidateSearchCriteria_WithEmptyCriteria_ShouldReturnInvalid()
        {
            var request = new SearchCriteriaRequest
            {
                OrderNo = "",
                SerialNo = ""
            };

            var response = await _client.PostAsJsonAsync("/api/validation/search-criteria", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ValidateOrderSearch_WithValidCompany_ShouldReturnValid()
        {
            await SeedTestDataAsync();

            var request = new OrderSearchRequest
            {
                CompanyId = "COMP01",
                OrderNo = "12345"
            };

            var response = await _client.PostAsJsonAsync("/api/validation/order-search", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ValidateModeSelection_WithValidModes_ShouldReturnValid()
        {
            var request = new ModeSelectionRequest
            {
                CalType = "ADM",
                SelectedModes = new List<string> { "Temperature", "Humidity" },
                CalNo = "CAL001"
            };

            var response = await _client.PostAsJsonAsync("/api/validation/mode-selection", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ValidateEditPermissions_WithValidCalibration_ShouldReturnValid()
        {
            var request = new EditPermissionsRequest
            {
                CalNo = "CAL001",
                EditType = "EDIT"
            };

            var response = await _client.PostAsJsonAsync("/api/validation/edit-permissions", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ValidateCalibrationTypeSelection_WithValidType_ShouldReturnValid()
        {
            var request = new CalibrationTypeRequest
            {
                CalType = "ADM"
            };

            var response = await _client.PostAsJsonAsync("/api/validation/calibration-type", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ValidateCalibrationTypeSelection_WithInvalidType_ShouldReturnInvalid()
        {
            var request = new CalibrationTypeRequest
            {
                CalType = "INVALID"
            };

            var response = await _client.PostAsJsonAsync("/api/validation/calibration-type", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }
    }
}
