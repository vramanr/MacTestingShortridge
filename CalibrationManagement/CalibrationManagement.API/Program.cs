using CalibrationManagement.Infrastructure.Data;
using CalibrationManagement.Application.Services;
using CalibrationManagement.Application.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CalibrationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(CalibrationMappingProfile));

builder.Services.AddScoped<ICalibrationWorkflowService, CalibrationWorkflowService>();
builder.Services.AddScoped<ICalibrationCalculationService, CalibrationCalculationService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IToleranceService, ToleranceService>();
builder.Services.AddScoped<IReportGenerationService, ReportGenerationService>();
builder.Services.AddScoped<IVfpBusinessLogicService, VfpBusinessLogicService>();
builder.Services.AddScoped<IMultiModeCalibrationService, MultiModeCalibrationService>();
builder.Services.AddScoped<IFormValidationService, FormValidationService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

            app.Run();
        }
    }
}
