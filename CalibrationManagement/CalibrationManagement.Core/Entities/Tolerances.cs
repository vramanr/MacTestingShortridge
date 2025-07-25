using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Core.Entities
{
    [Table("tolerances")]
    public class Tolerances
    {
        [Key]
        [Column("tolerance_id")]
        public Guid ToleranceId { get; set; } = Guid.NewGuid();

        [Column("cal_type")]
        [StringLength(50)]
        public string? CalType { get; set; }

        [Column("range_min")]
        [Precision(15, 6)]
        public decimal? RangeMin { get; set; }

        [Column("range_max")]
        [Precision(15, 6)]
        public decimal? RangeMax { get; set; }

        [Column("tolerance_value")]
        [Precision(15, 6)]
        public decimal? ToleranceValue { get; set; }

        [Column("tolerance_type")]
        [StringLength(20)]
        public string? ToleranceType { get; set; }

        [Column("units")]
        [StringLength(20)]
        public string? Units { get; set; }

        [Column("active")]
        public bool Active { get; set; } = true;

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;
    }
}
