using System.ComponentModel.DataAnnotations;

namespace CalibrationManagement.Application.DTOs
{
    public class ToleranceDto
    {
        public Guid ToleranceId { get; set; }
        
        [StringLength(50)]
        public string? CalType { get; set; }
        
        [StringLength(50)]
        public string? Mode { get; set; }
        
        public decimal? RangeMin { get; set; }
        
        public decimal? RangeMax { get; set; }
        
        [StringLength(20)]
        public string? ToleranceType { get; set; }
        
        public decimal? ToleranceValue { get; set; }
        
        public decimal? PercentTolerance { get; set; }
        
        public decimal? ConstantTolerance { get; set; }
        
        [StringLength(20)]
        public string? Units { get; set; }
        
        public bool Active { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }

    public class CreateToleranceDto
    {
        [Required]
        [StringLength(50)]
        public string CalType { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Mode { get; set; }
        
        [Required]
        public decimal RangeMin { get; set; }
        
        [Required]
        public decimal RangeMax { get; set; }
        
        [Required]
        [StringLength(20)]
        public string ToleranceType { get; set; } = string.Empty;
        
        public decimal? ToleranceValue { get; set; }
        
        public decimal? PercentTolerance { get; set; }
        
        public decimal? ConstantTolerance { get; set; }
        
        [StringLength(20)]
        public string? Units { get; set; }
        
        public bool Active { get; set; } = true;
    }

    public class UpdateToleranceDto
    {
        [Required]
        public Guid ToleranceId { get; set; }
        
        [StringLength(50)]
        public string? CalType { get; set; }
        
        [StringLength(50)]
        public string? Mode { get; set; }
        
        public decimal? RangeMin { get; set; }
        
        public decimal? RangeMax { get; set; }
        
        [StringLength(20)]
        public string? ToleranceType { get; set; }
        
        public decimal? ToleranceValue { get; set; }
        
        public decimal? PercentTolerance { get; set; }
        
        public decimal? ConstantTolerance { get; set; }
        
        [StringLength(20)]
        public string? Units { get; set; }
        
        public bool Active { get; set; }
    }
}
