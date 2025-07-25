using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CalibrationManagement.Infrastructure.Data;

namespace CalibrationManagement.Tests
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly string _databaseName = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptorsToRemove = services.Where(d => 
                    d.ServiceType == typeof(DbContextOptions<CalibrationDbContext>) ||
                    d.ServiceType == typeof(CalibrationDbContext) ||
                    (d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)) ||
                    d.ImplementationType?.FullName?.Contains("EntityFramework") == true ||
                    d.ImplementationType?.FullName?.Contains("Npgsql") == true ||
                    d.ServiceType.FullName?.Contains("EntityFramework") == true ||
                    d.ServiceType.FullName?.Contains("Npgsql") == true)
                    .ToList();

                foreach (var descriptor in descriptorsToRemove)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<CalibrationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName: _databaseName);
                    options.EnableSensitiveDataLogging();
                });
            });

            builder.UseEnvironment("Testing");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    using var scope = Services.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<CalibrationDbContext>();
                    context.Database.EnsureDeleted();
                }
                catch (ObjectDisposedException)
                {
                }
            }
            base.Dispose(disposing);
        }
    }
}
