using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Core.Entities
{
    [Table("cal_standards")]
    public class CalStandards
    {
        [Key]
        [Column("cal_std_id")]
        public Guid CalStdId { get; set; } = Guid.NewGuid();

        [Column("cal_id")]
        public Guid CalId { get; set; }

        [Column("std_serial_no")]
        [StringLength(100)]
        public string? StdSerialNo { get; set; }

        [Column("std_model")]
        [StringLength(100)]
        public string? StdModel { get; set; }

        [Column("std_cal_date")]
        public DateTime? StdCalDate { get; set; }

        [Column("std_due_date")]
        public DateTime? StdDueDate { get; set; }

        [Column("std_uncertainty")]
        [Precision(15, 6)]
        public decimal? StdUncertainty { get; set; }

        [Column("std_type")]
        [StringLength(50)]
        public string? StdType { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;

        [ForeignKey("CalId")]
        public virtual CalInfo CalInfo { get; set; } = null!;
    }
}
