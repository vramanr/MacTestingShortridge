using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalibrationManagement.Core.Entities
{
    [Table("cal_techs")]
    public class CalTechs
    {
        [Key]
        [Column("cal_tech_id")]
        public Guid CalTechId { get; set; } = Guid.NewGuid();

        [Column("tech_name")]
        [StringLength(100)]
        public string TechName { get; set; } = string.Empty;

        [Column("tech_initials")]
        [StringLength(10)]
        public string? TechInitials { get; set; }

        [Column("active")]
        public bool Active { get; set; } = true;

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;
    }
}
