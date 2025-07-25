using CalibrationManagement.Core.Entities;
using CalibrationManagement.Application.DTOs;
using CalibrationManagement.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

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
                .FirstOrDefaultAsync(c => c.CalNo == calNo.ToString());

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
            using var memoryStream = new MemoryStream();
            using var writer = new PdfWriter(memoryStream);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            switch (templateName)
            {
                case "CalibrationCertificate":
                    await GenerateCalibrationCertificatePdf(document, (ReportDataDto)data);
                    break;
                case "StandardsReport":
                    await GenerateStandardsReportPdf(document, (List<CalStandardsReportDto>)data);
                    break;
                case "CoverSheet":
                    await GenerateCoverSheetPdf(document, (ReportDataDto)data);
                    break;
                default:
                    document.Add(new Paragraph($"Unknown template: {templateName}"));
                    break;
            }

            document.Close();
            return memoryStream.ToArray();
        }

        private async Task GenerateCalibrationCertificatePdf(Document document, ReportDataDto reportData)
        {
            document.Add(new Paragraph(reportData.ReportTitle)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(16)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetMarginBottom(20));

            document.Add(new Paragraph($"Company: {reportData.CompanyName}")
                .SetMarginBottom(5));
            document.Add(new Paragraph($"Location: {reportData.City}, {reportData.State}")
                .SetMarginBottom(5));
            document.Add(new Paragraph($"Model Number: {reportData.ModelNumber}")
                .SetMarginBottom(5));
            document.Add(new Paragraph($"Serial Number: {reportData.SerialNumber}")
                .SetMarginBottom(5));
            document.Add(new Paragraph($"Calibration Date: {reportData.CalibrationDate}")
                .SetMarginBottom(20));

            document.Add(new Paragraph("Test Information:")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetMarginBottom(10));
            document.Add(new Paragraph($"Procedure Used: Standard Calibration Procedure")
                .SetMarginBottom(5));
            document.Add(new Paragraph($"Revision: Rev 1.0")
                .SetMarginBottom(5));
            document.Add(new Paragraph($"Date: {DateTime.Now:MM/dd/yyyy}")
                .SetMarginBottom(5));
            document.Add(new Paragraph($"Calibrated By: {reportData.TestBy}")
                .SetMarginBottom(20));

            document.Add(new Paragraph("Readings:")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetMarginBottom(10));

            var table = new Table(6);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Approximate Set Point").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)).SetFontSize(10)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Standard Reading").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)).SetFontSize(10)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Test Meter Reading").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)).SetFontSize(10)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Allowed Deviation").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)).SetFontSize(10)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Actual Deviation").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)).SetFontSize(10)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Tolerance").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)).SetFontSize(10)));

            foreach (var dataPoint in reportData.CalibrationData)
            {
                table.AddCell(new Cell().Add(new Paragraph(dataPoint.SetPoint).SetFontSize(9)));
                table.AddCell(new Cell().Add(new Paragraph(dataPoint.StandardReading).SetFontSize(9)));
                table.AddCell(new Cell().Add(new Paragraph(dataPoint.TestReading).SetFontSize(9)));
                table.AddCell(new Cell().Add(new Paragraph(dataPoint.AllowedDeviation).SetFontSize(9)));
                table.AddCell(new Cell().Add(new Paragraph(dataPoint.ActualDeviation).SetFontSize(9)));
                table.AddCell(new Cell().Add(new Paragraph(dataPoint.Tolerance).SetFontSize(9)));
            }

            document.Add(table);

            document.Add(new Paragraph("Approved By:________________________ Title: __________________  Date: ________")
                .SetMarginTop(40));
        }

        private async Task GenerateStandardsReportPdf(Document document, List<CalStandardsReportDto> standardsData)
        {
            document.Add(new Paragraph("Calibration Standards Table - Part One")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(16)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetMarginBottom(20));

            var table = new Table(13);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            string[] headers = { "Name", "Sensor", "Model", "Serial#", "Cal Date", "Cal Due Date", 
                               "Range", "Units", "Accuracy", "Uncertainty", "Mfg By", "Cal By", "Cal Interval" };
            
            foreach (var header in headers)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(header).SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)).SetFontSize(8)));
            }

            foreach (var standard in standardsData)
            {
                table.AddCell(new Cell().Add(new Paragraph(standard.Name).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.Sensor).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.Model).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.SerialNumber).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.CalibrationDate).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.DueDate).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.Range).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.Units).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.Accuracy).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.Uncertainty).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.Manufacturer).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.CalibratedBy).SetFontSize(7)));
                table.AddCell(new Cell().Add(new Paragraph(standard.CalibrationInterval).SetFontSize(7)));
            }

            document.Add(table);

            document.Add(new Paragraph($"Page 1")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetMarginTop(20));
        }

        private async Task GenerateCoverSheetPdf(Document document, ReportDataDto reportData)
        {
            document.Add(new Paragraph("Shortridge Instruments, Inc.")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetMarginBottom(10));

            document.Add(new Paragraph("Calibration Report")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(14)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetMarginBottom(5));

            document.Add(new Paragraph("for")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(12)
                .SetMarginBottom(30));

            document.Add(new Paragraph(reportData.CompanyName)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(16)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetMarginBottom(5));

            document.Add(new Paragraph($"{reportData.City}, {reportData.State}")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(12)
                .SetMarginBottom(40));

            document.Add(new Paragraph($"Model Number: {reportData.ModelNumber}")
                .SetFontSize(12)
                .SetMarginBottom(10));

            document.Add(new Paragraph($"Serial Number: {reportData.SerialNumber}")
                .SetFontSize(12)
                .SetMarginBottom(10));

            document.Add(new Paragraph($"Calibration Date: {reportData.CalibrationDate}")
                .SetFontSize(12)
                .SetMarginBottom(10));
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
