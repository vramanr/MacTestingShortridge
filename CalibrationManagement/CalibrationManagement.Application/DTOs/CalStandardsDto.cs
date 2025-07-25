using System.ComponentModel.DataAnnotations;

namespace CalibrationManagement.Application.DTOs
{
    public class CalStandardsDto
    {
        public Guid CalStandardId { get; set; }
        
        public Guid CalId { get; set; }
        
        [StringLength(50)]
        public string? StandardId { get; set; }
        
        [StringLength(100)]
        public string? StandardDescription { get; set; }
        
        [StringLength(50)]
        public string? SerialNo { get; set; }
        
        [StringLength(50)]
        public string? ModelNo { get; set; }
        
        [StringLength(50)]
        public string? Manufacturer { get; set; }
        
        public DateTime? CalDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [StringLength(50)]
        public string? CertificateNo { get; set; }
        
        [StringLength(100)]
        public string? CalLab { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }

    public class CreateCalStandardsDto
    {
        [Required]
        public Guid CalId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string StandardId { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? StandardDescription { get; set; }
        
        [StringLength(50)]
        public string? SerialNo { get; set; }
        
        [StringLength(50)]
        public string? ModelNo { get; set; }
        
        [StringLength(50)]
        public string? Manufacturer { get; set; }
        
        public DateTime? CalDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [StringLength(50)]
        public string? CertificateNo { get; set; }
        
        [StringLength(100)]
        public string? CalLab { get; set; }
    }

    public class UpdateCalStandardsDto
    {
        [Required]
        public Guid CalStandardId { get; set; }
        
        [Required]
        public Guid CalId { get; set; }
        
        [StringLength(50)]
        public string? StandardId { get; set; }
        
        [StringLength(100)]
        public string? StandardDescription { get; set; }
        
        [StringLength(50)]
        public string? SerialNo { get; set; }
        
        [StringLength(50)]
        public string? ModelNo { get; set; }
        
        [StringLength(50)]
        public string? Manufacturer { get; set; }
        
        public DateTime? CalDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [StringLength(50)]
        public string? CertificateNo { get; set; }
        
        [StringLength(100)]
        public string? CalLab { get; set; }
    }
}
