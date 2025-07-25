using System.ComponentModel.DataAnnotations;

namespace CalibrationManagement.Application.DTOs
{
    public class CalInfoDto
    {
        public Guid CalId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string CalNo { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? CoId { get; set; }
        
        [StringLength(50)]
        public string? OrderNo { get; set; }
        
        [StringLength(100)]
        public string? SerialNo { get; set; }
        
        [StringLength(100)]
        public string? ModelNo { get; set; }
        
        public DateTime? CalDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [StringLength(50)]
        public string? CalType { get; set; }
        
        [StringLength(50)]
        public string? CalTech { get; set; }
        
        [StringLength(20)]
        public string? CalStatus { get; set; }
        
        public decimal? Temperature { get; set; }
        
        public decimal? Humidity { get; set; }
        
        public decimal? Pressure { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public CompanyDto? Company { get; set; }
        
        public List<CalDataDto> CalData { get; set; } = new();
        
        public List<CalStandardsDto> CalStandards { get; set; } = new();
    }

    public class CreateCalInfoDto
    {
        [StringLength(20)]
        public string? CoId { get; set; }
        
        [StringLength(50)]
        public string? OrderNo { get; set; }
        
        [Required]
        [StringLength(100)]
        public string SerialNo { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string ModelNo { get; set; } = string.Empty;
        
        public DateTime? CalDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [Required]
        [StringLength(50)]
        public string CalType { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? CalTech { get; set; }
        
        [StringLength(20)]
        public string CalStatus { get; set; } = "PENDING";
        
        public decimal? Temperature { get; set; }
        
        public decimal? Humidity { get; set; }
        
        public decimal? Pressure { get; set; }
        
        public string? Notes { get; set; }
    }

    public class UpdateCalInfoDto
    {
        [Required]
        public Guid CalId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string CalNo { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? CoId { get; set; }
        
        [StringLength(50)]
        public string? OrderNo { get; set; }
        
        [StringLength(100)]
        public string? SerialNo { get; set; }
        
        [StringLength(100)]
        public string? ModelNo { get; set; }
        
        public DateTime? CalDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [StringLength(50)]
        public string? CalType { get; set; }
        
        [StringLength(50)]
        public string? CalTech { get; set; }
        
        [StringLength(20)]
        public string? CalStatus { get; set; }
        
        public decimal? Temperature { get; set; }
        
        public decimal? Humidity { get; set; }
        
        public decimal? Pressure { get; set; }
        
        public string? Notes { get; set; }
    }
}
