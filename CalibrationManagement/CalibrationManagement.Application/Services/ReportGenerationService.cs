using CalibrationManagement.Core.Entities;
using CalibrationManagement.Application.DTOs;
using CalibrationManagement.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CalibrationManagement.Application.Services
{
    public interface IReportGenerationService
    {
        Task<byte[]> GenerateCalibrationCertificateAsync(int calNo);
        Task<byte[]> GenerateStandardsReportAsync();
        Task<byte[]> GenerateCoverSheetAsync(int calNo);
        Task<ReportDataDto> GetCalibrationReportDataAsync(int calNo);
        Task<List<CalStandardsReportDto>> GetStandardsReportDataAsync();
    }

    public class ReportGenerationService : IReportGenerationService
    {
        private readonly ILogger<ReportGenerationService> _logger;
        private readonly CalibrationDbContext _context;

        public ReportGenerationService(
            ILogger<ReportGenerationService> logger,
            CalibrationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<byte[]> GenerateCalibrationCertificateAsync(int calNo)
        {
            try
            {
                var reportData = await GetCalibrationReportDataAsync(calNo);
                return await GeneratePdfFromTemplate("CalibrationCertificate", reportData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating calibration certificate for CalNo: {CalNo}", calNo);
                throw;
            }
        }

        public async Task<byte[]> GenerateStandardsReportAsync()
        {
            try
            {
                var reportData = await GetStandardsReportDataAsync();
                return await GeneratePdfFromTemplate("StandardsReport", reportData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating standards report");
                throw;
            }
        }

        public async Task<byte[]> GenerateCoverSheetAsync(int calNo)
        {
            try
            {
                var reportData = await GetCalibrationReportDataAsync(calNo);
                return await GeneratePdfFromTemplate("CoverSheet", reportData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating cover sheet for CalNo: {CalNo}", calNo);
                throw;
            }
        }

        public async Task<ReportDataDto> GetCalibrationReportDataAsync(int calNo)
        {
            var calInfo = await _context.CalInfo
                .Include(c => c.Company)
                .Include(c => c.CalData)
                .FirstOrDefaultAsync(c => c.CalNo == calNo);

            if (calInfo == null)
                throw new ArgumentException($"Calibration record not found for CalNo: {calNo}");

            var reportData = new ReportDataDto
            {
                CalNo = calInfo.CalNo.ToString(),
                CompanyName = calInfo.Company?.CoName ?? "",
                City = calInfo.Company?.SCity ?? "",
                State = calInfo.Company?.SState ?? "",
                ModelNumber = calInfo.AsrcdModNo ?? "",
                SerialNumber = calInfo.SerialNo ?? "",
                CalibrationDate = calInfo.CalDate?.ToString("MM/dd/yyyy") ?? "",
                TestType = calInfo.TestType ?? "",
                TestBy = calInfo.TestBy ?? "",
                ReportTitle = "AIRDATA MULTIMETER/FLOWMETER CERTIFICATE OF CALIBRATION",
                CalibrationData = calInfo.CalData?.Select(cd => new CalibrationDataDto
                {
                    SetPoint = cd.SetPoint?.ToString("F4") ?? "",
                    StandardReading = cd.StdRead?.ToString("F4") ?? "",
                    TestReading = cd.TestRead?.ToString("F4") ?? "",
                    AllowedDeviation = cd.AllowDev?.ToString("F4") ?? "",
                    ActualDeviation = cd.ActualDev?.ToString("F4") ?? "",
                    Tolerance = cd.ReportTol ?? ""
                }).ToList() ?? new List<CalibrationDataDto>()
            };

            return reportData;
        }

        public async Task<List<CalStandardsReportDto>> GetStandardsReportDataAsync()
        {
            var standards = await _context.CalStandards
                .OrderBy(s => s.Name)
                .ToListAsync();

            return standards.Select(s => new CalStandardsReportDto
            {
                Name = s.Name ?? "",
                Sensor = s.Sensor ?? "",
                Model = s.ModelNo ?? "",
                SerialNumber = s.SerialNo ?? "",
                CalibrationDate = s.CalDate?.ToString("MM/dd/yyyy") ?? "",
                DueDate = s.CalDueDate?.ToString("MM/dd/yyyy") ?? "",
                Range = s.Range ?? "",
                Units = s.Units ?? "",
                Accuracy = s.RtAccPfs ?? "",
                Uncertainty = s.UncertFs ?? "",
                Manufacturer = s.MfgBy ?? "",
                CalibratedBy = s.CalBy ?? "",
                CalibrationInterval = s.CalInterval?.ToString() ?? ""
            }).ToList();
        }

        private async Task<byte[]> GeneratePdfFromTemplate(string templateName, object data)
        {
            var placeholder = $"PDF Report: {templateName}\nGenerated: {DateTime.Now}\nData: {System.Text.Json.JsonSerializer.Serialize(data)}";
            return Encoding.UTF8.GetBytes(placeholder);
        }
    }

    public class ReportDataDto
    {
        public string CalNo { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string ModelNumber { get; set; } = "";
        public string SerialNumber { get; set; } = "";
        public string CalibrationDate { get; set; } = "";
        public string TestType { get; set; } = "";
        public string TestBy { get; set; } = "";
        public string ReportTitle { get; set; } = "";
        public List<CalibrationDataDto> CalibrationData { get; set; } = new();
    }

    public class CalibrationDataDto
    {
        public string SetPoint { get; set; } = "";
        public string StandardReading { get; set; } = "";
        public string TestReading { get; set; } = "";
        public string AllowedDeviation { get; set; } = "";
        public string ActualDeviation { get; set; } = "";
        public string Tolerance { get; set; } = "";
    }

    public class CalStandardsReportDto
    {
        public string Name { get; set; } = "";
        public string Sensor { get; set; } = "";
        public string Model { get; set; } = "";
        public string SerialNumber { get; set; } = "";
        public string CalibrationDate { get; set; } = "";
        public string DueDate { get; set; } = "";
        public string Range { get; set; } = "";
        public string Units { get; set; } = "";
        public string Accuracy { get; set; } = "";
        public string Uncertainty { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string CalibratedBy { get; set; } = "";
        public string CalibrationInterval { get; set; } = "";
    }
}
