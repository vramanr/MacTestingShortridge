using System.ComponentModel.DataAnnotations;
using CalibrationManagement.Application.Validators;

namespace CalibrationManagement.Application.DTOs
{
    public class CalDataDto
    {
        public Guid CalDataId { get; set; }
        
        public Guid CalId { get; set; }
        
        [StringLength(50)]
        public string? CalNo { get; set; }
        
        [StringLength(50)]
        public string? Mode { get; set; }
        
        public decimal? SetPoint { get; set; }
        
        public decimal? ActualReading { get; set; }
        
        public decimal? Deviation { get; set; }
        
        public decimal? PercentDeviation { get; set; }
        
        [ToleranceValue]
        public decimal? Tolerance { get; set; }
        
        [StringLength(10)]
        public string? PassFail { get; set; }
        
        [StringLength(20)]
        public string? Units { get; set; }
        
        public int? SequenceNo { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }

    public class CreateCalDataDto
    {
        [Required]
        public Guid CalId { get; set; }
        
        [StringLength(50)]
        public string? Mode { get; set; }
        
        [Required]
        [CalibrationReading]
        public decimal SetPoint { get; set; }
        
        [Required]
        [CalibrationReading]
        public decimal ActualReading { get; set; }
        
        [ToleranceValue]
        public decimal? Tolerance { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Units { get; set; } = string.Empty;
        
        public int? SequenceNo { get; set; }
    }

    public class UpdateCalDataDto
    {
        [Required]
        public Guid CalDataId { get; set; }
        
        [Required]
        public Guid CalId { get; set; }
        
        [StringLength(50)]
        public string? Mode { get; set; }
        
        public decimal? SetPoint { get; set; }
        
        public decimal? ActualReading { get; set; }
        
        public decimal? Deviation { get; set; }
        
        public decimal? PercentDeviation { get; set; }
        
        [ToleranceValue]
        public decimal? Tolerance { get; set; }
        
        [StringLength(10)]
        public string? PassFail { get; set; }
        
        [StringLength(20)]
        public string? Units { get; set; }
        
        public int? SequenceNo { get; set; }
    }
}
