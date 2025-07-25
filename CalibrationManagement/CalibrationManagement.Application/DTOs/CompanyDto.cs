using System.ComponentModel.DataAnnotations;

namespace CalibrationManagement.Application.DTOs
{
    public class CompanyDto
    {
        public Guid CompanyId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string CoId { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string CoName { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Address1 { get; set; }
        
        [StringLength(100)]
        public string? Address2 { get; set; }
        
        [StringLength(200)]
        public string? Address { get; set; }
        
        [StringLength(50)]
        public string? City { get; set; }
        
        [StringLength(20)]
        public string? State { get; set; }
        
        [StringLength(20)]
        public string? ZipCode { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(20)]
        public string? Fax { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(100)]
        public string? Website { get; set; }
        
        public bool Active { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public List<ContactDto> Contacts { get; set; } = new();
        
        public List<ModelNoDto> ModelNumbers { get; set; } = new();
    }

    public class CreateCompanyDto
    {
        [Required]
        [StringLength(20)]
        public string CoId { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string CoName { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Address1 { get; set; }
        
        [StringLength(100)]
        public string? Address2 { get; set; }
        
        [StringLength(200)]
        public string? Address { get; set; }
        
        [StringLength(50)]
        public string? City { get; set; }
        
        [StringLength(20)]
        public string? State { get; set; }
        
        [StringLength(20)]
        public string? ZipCode { get; set; }
        
        [StringLength(20)]
        public string? Zip { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(20)]
        public string? Fax { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(100)]
        public string? Website { get; set; }
        
        public bool Active { get; set; } = true;
    }

    public class UpdateCompanyDto
    {
        [Required]
        public Guid CompanyId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string CoId { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string CoName { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Address1 { get; set; }
        
        [StringLength(100)]
        public string? Address2 { get; set; }
        
        [StringLength(200)]
        public string? Address { get; set; }
        
        [StringLength(50)]
        public string? City { get; set; }
        
        [StringLength(20)]
        public string? State { get; set; }
        
        [StringLength(20)]
        public string? ZipCode { get; set; }
        
        [StringLength(20)]
        public string? Zip { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(20)]
        public string? Fax { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(100)]
        public string? Website { get; set; }
        
        public bool Active { get; set; }
    }
}
