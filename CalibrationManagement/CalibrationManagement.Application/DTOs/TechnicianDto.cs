using System.ComponentModel.DataAnnotations;

namespace CalibrationManagement.Application.DTOs
{
    public class TechnicianDto
    {
        public Guid CalTechId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string TechName { get; set; } = string.Empty;
        
        [StringLength(10)]
        public string? TechInitials { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(50)]
        public string? Department { get; set; }
        
        public bool Active { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }

    public class CreateTechnicianDto
    {
        [Required]
        [StringLength(50)]
        public string TechName { get; set; } = string.Empty;
        
        [StringLength(10)]
        public string? TechInitials { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(50)]
        public string? Department { get; set; }
        
        public bool Active { get; set; } = true;
    }

    public class UpdateTechnicianDto
    {
        [Required]
        public Guid CalTechId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string TechName { get; set; } = string.Empty;
        
        [StringLength(10)]
        public string? TechInitials { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(50)]
        public string? Department { get; set; }
        
        public bool Active { get; set; }
    }
}
