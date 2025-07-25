using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalibrationManagement.Core.Entities
{
    [Table("cal_setup")]
    public class CalSetup
    {
        [Key]
        [Column("cal_setup_id")]
        public Guid CalSetupId { get; set; } = Guid.NewGuid();

        [Column("cal_type")]
        [StringLength(50)]
        public string? CalType { get; set; }

        [Column("setup_name")]
        [StringLength(100)]
        public string? SetupName { get; set; }

        [Column("setup_description")]
        [StringLength(500)]
        public string? SetupDescription { get; set; }

        [Column("default_points")]
        public int? DefaultPoints { get; set; }

        [Column("default_units")]
        [StringLength(20)]
        public string? DefaultUnits { get; set; }

        [Column("mode")]
        [StringLength(50)]
        public string? Mode { get; set; }

        [Column("cal_std")]
        [StringLength(100)]
        public string? CalStd { get; set; }

        [Column("cal_std2")]
        [StringLength(100)]
        public string? CalStd2 { get; set; }

        [Column("sort_order")]
        public int? SortOrder { get; set; }

        [Column("active")]
        public bool Active { get; set; } = true;

        public bool IsActive => Active;

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;
    }
}
