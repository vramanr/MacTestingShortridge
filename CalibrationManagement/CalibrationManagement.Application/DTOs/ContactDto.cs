using System.ComponentModel.DataAnnotations;

namespace CalibrationManagement.Application.DTOs
{
    public class ContactDto
    {
        public Guid ContactId { get; set; }
        
        [StringLength(20)]
        public string? CoId { get; set; }
        
        [StringLength(100)]
        public string? ContactName { get; set; }
        
        [StringLength(50)]
        public string? Title { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(20)]
        public string? Fax { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        public bool Active { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }

    public class CreateContactDto
    {
        [Required]
        [StringLength(20)]
        public string CoId { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string ContactName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Title { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(20)]
        public string? Fax { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        public bool Active { get; set; } = true;
    }

    public class ModelNoDto
    {
        public Guid ModelNoId { get; set; }
        
        [StringLength(20)]
        public string? CoId { get; set; }
        
        [StringLength(100)]
        public string? ModelNo { get; set; }
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        [StringLength(50)]
        public string? CalType { get; set; }
        
        public bool Active { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }

    public class CreateModelNoDto
    {
        [Required]
        [StringLength(20)]
        public string CoId { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string ModelNo { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        [StringLength(50)]
        public string? CalType { get; set; }
        
        public bool Active { get; set; } = true;
    }
}
