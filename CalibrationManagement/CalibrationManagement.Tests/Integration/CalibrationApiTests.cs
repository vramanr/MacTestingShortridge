using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text;
using Xunit;
using FluentAssertions;
using CalibrationManagement.Infrastructure.Data;
using CalibrationManagement.Application.DTOs;
using CalibrationManagement.Core.Entities;

namespace CalibrationManagement.Tests.Integration
{
    public class CalibrationApiTests : IClassFixture<TestWebApplicationFactory<CalibrationManagement.API.Program>>
    {
        private readonly TestWebApplicationFactory<CalibrationManagement.API.Program> _factory;
        private readonly HttpClient _client;

        public CalibrationApiTests(TestWebApplicationFactory<CalibrationManagement.API.Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CreateCalibration_ShouldReturnCreatedCalibration()
        {
            var createDto = new CreateCalInfoDto
            {
                SerialNo = "TEST123",
                ModelNumber = "MODEL123",
                CalType = "PRESSURE",
                CalStatus = "PENDING"
            };

            var json = JsonSerializer.Serialize(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/calibration", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CalInfoDto>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.Should().NotBeNull();
            result!.SerialNo.Should().Be("TEST123");
            result.ModelNumber.Should().Be("MODEL123");
            result.CalType.Should().Be("PRESSURE");
        }

        [Fact]
        public async Task GetCalibration_ShouldReturnCalibrationById()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();
            
            var calInfo = new CalInfo
            {
                CalNo = "TEST001",
                SerialNo = "SN123",
                ModelNumber = "MODEL123",
                CalType = "PRESSURE"
            };
            context.CalInfo.Add(calInfo);
            await context.SaveChangesAsync();

            var response = await _client.GetAsync($"/api/calibration/{calInfo.CalId}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CalInfoDto>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.Should().NotBeNull();
            result!.CalNo.Should().Be("TEST001");
        }

        [Fact]
        public async Task SearchCalibrations_ShouldReturnMatchingResults()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();
            
            var calInfo1 = new CalInfo
            {
                CalNo = "SEARCH001",
                SerialNo = "FINDME123",
                ModelNumber = "MODEL123",
                CalType = "PRESSURE"
            };
            var calInfo2 = new CalInfo
            {
                CalNo = "OTHER001",
                SerialNo = "OTHER123",
                ModelNumber = "MODEL456",
                CalType = "TEMPERATURE"
            };
            context.CalInfo.AddRange(calInfo1, calInfo2);
            await context.SaveChangesAsync();

            var response = await _client.GetAsync("/api/calibration/search?searchTerm=FINDME");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var results = JsonSerializer.Deserialize<List<CalInfoDto>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            results.Should().NotBeNull();
            results!.Should().HaveCount(1);
            results.First().SerialNo.Should().Be("FINDME123");
        }

        [Fact]
        public async Task ValidateCalibrationEntry_ShouldReturnValidationResult()
        {
            var validationRequest = new
            {
                CalInfo = new
                {
                    CalType = "PRESSURE",
                    SerialNo = "TEST123",
                    ModelNumber = "MODEL123"
                },
                CalData = new object[] { }
            };

            var json = JsonSerializer.Serialize(validationRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/validation/calibration-entry", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(responseContent);

            result.GetProperty("isValid").GetBoolean().Should().BeTrue();
        }
    }
}
