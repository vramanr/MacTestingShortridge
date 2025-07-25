using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalibrationManagement.Core.Entities
{
    [Table("contact")]
    public class Contact
    {
        [Key]
        [Column("contact_id")]
        public Guid ContactId { get; set; } = Guid.NewGuid();

        [Column("co_id")]
        [StringLength(20)]
        public string? CoId { get; set; }

        [Column("first_name")]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Column("contact_name")]
        [StringLength(100)]
        public string? ContactName { get; set; }

        [Column("is_primary")]
        public bool? IsPrimary { get; set; }

        [Column("last_name")]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Column("title")]
        [StringLength(100)]
        public string? Title { get; set; }

        [Column("phone")]
        [StringLength(30)]
        public string? Phone { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Column("primary_contact")]
        public bool PrimaryContact { get; set; } = false;

        [Column("active")]
        public bool Active { get; set; } = true;

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;

        public virtual Company? Company { get; set; }
    }
}
