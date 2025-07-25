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
    public class CompanyControllerTests : IClassFixture<TestWebApplicationFactory<CalibrationManagement.API.Program>>
    {
        private readonly TestWebApplicationFactory<CalibrationManagement.API.Program> _factory;
        private readonly HttpClient _client;

        public CompanyControllerTests(TestWebApplicationFactory<CalibrationManagement.API.Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        private async Task SeedTestDataAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            context.Company.AddRange(
                new Company 
                { 
                    CoId = "COMP01", 
                    CoName = "Test Company 1",
                    Address = "123 Test St",
                    City = "Test City",
                    State = "TS",
                    Zip = "12345"
                },
                new Company 
                { 
                    CoId = "COMP02", 
                    CoName = "Test Company 2",
                    Address = "456 Test Ave",
                    City = "Test Town",
                    State = "TT",
                    Zip = "67890"
                }
            );

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetCompanies_ShouldReturnAllCompanies()
        {
            await SeedTestDataAsync();

            var response = await _client.GetAsync("/api/company");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var companies = await response.Content.ReadFromJsonAsync<List<CompanyDto>>();
            companies.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetCompany_WithValidId_ShouldReturnCompany()
        {
            await SeedTestDataAsync();

            var response = await _client.GetAsync("/api/company/by-co-id/COMP01");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var company = await response.Content.ReadFromJsonAsync<CompanyDto>();
            company.Should().NotBeNull();
            company!.CoId.Should().Be("COMP01");
            company.CoName.Should().Be("Test Company 1");
        }

        [Fact]
        public async Task GetCompany_WithInvalidId_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync("/api/company/by-co-id/INVALID");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateCompany_WithValidData_ShouldCreateCompany()
        {
            var newCompany = new CreateCompanyDto
            {
                CoId = "COMP03",
                CoName = "Test Company 3",
                Address = "789 Test Blvd",
                City = "Test Village",
                State = "TV",
                Zip = "11111"
            };

            var response = await _client.PostAsJsonAsync("/api/company", newCompany);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var createdCompany = await response.Content.ReadFromJsonAsync<CompanyDto>();
            createdCompany.Should().NotBeNull();
            createdCompany!.CoId.Should().Be("COMP03");
            createdCompany.CoName.Should().Be("Test Company 3");
        }

        [Fact]
        public async Task UpdateCompany_WithValidData_ShouldUpdateCompany()
        {
            await SeedTestDataAsync();

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();
            var company = await context.Company.FirstOrDefaultAsync(c => c.CoId == "COMP01");
            company.Should().NotBeNull();

            var updateCompany = new UpdateCompanyDto
            {
                CompanyId = company!.CompanyId,
                CoId = "COMP01",
                CoName = "Updated Test Company 1",
                Address = "123 Updated St",
                City = "Updated City",
                State = "UC",
                Zip = "54321"
            };

            var response = await _client.PutAsJsonAsync($"/api/company/{company.CompanyId}", updateCompany);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedCompany = await response.Content.ReadFromJsonAsync<CompanyDto>();
            updatedCompany.Should().NotBeNull();
            updatedCompany!.CoName.Should().Be("Updated Test Company 1");
            updatedCompany.Address.Should().Be("123 Updated St");
        }

        [Fact]
        public async Task DeleteCompany_WithValidId_ShouldDeleteCompany()
        {
            await SeedTestDataAsync();

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();
            var company = await context.Company.FirstOrDefaultAsync(c => c.CoId == "COMP02");
            company.Should().NotBeNull();

            var response = await _client.DeleteAsync($"/api/company/{company!.CompanyId}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync("/api/company/by-co-id/COMP02");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SearchCompanies_WithValidCriteria_ShouldReturnMatchingCompanies()
        {
            await SeedTestDataAsync();

            var response = await _client.GetAsync("/api/company/search?searchTerm=Test Company 1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var companies = await response.Content.ReadFromJsonAsync<List<CompanyDto>>();
            companies.Should().HaveCount(1);
            companies![0].CoName.Should().Be("Test Company 1");
        }
    }
}
