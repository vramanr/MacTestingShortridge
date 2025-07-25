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

        [Column("cal_standard_id")]
        public Guid CalStandardId { get; set; }

        [Column("standard_id")]
        [StringLength(50)]
        public string? StandardId { get; set; }

        [Column("standard_name")]
        [StringLength(200)]
        public string? StandardName { get; set; }

        [Column("name")]
        [StringLength(200)]
        public string? Name { get; set; }

        [Column("cal_id")]
        public Guid CalId { get; set; }

        [Column("std_serial_no")]
        [StringLength(100)]
        public string? StdSerialNo { get; set; }

        [Column("serial_no")]
        [StringLength(100)]
        public string? SerialNo { get; set; }

        [Column("std_model")]
        [StringLength(100)]
        public string? StdModel { get; set; }

        [Column("std_cal_date")]
        public DateTime? StdCalDate { get; set; }

        [Column("cal_date")]
        public DateTime? CalDate { get; set; }

        [Column("std_due_date")]
        public DateTime? StdDueDate { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column("std_uncertainty")]
        [Precision(15, 6)]
        public decimal? StdUncertainty { get; set; }

        [Column("uncertainty")]
        [Precision(15, 6)]
        public decimal? Uncertainty { get; set; }

        [Column("std_type")]
        [StringLength(50)]
        public string? StdType { get; set; }

        [Column("sensor")]
        [StringLength(100)]
        public string? Sensor { get; set; }

        [Column("model_no")]
        [StringLength(100)]
        public string? ModelNo { get; set; }

        [Column("cal_due_date")]
        public DateTime? CalDueDate { get; set; }

        [Column("range")]
        [StringLength(100)]
        public string? Range { get; set; }

        [Column("units")]
        [StringLength(20)]
        public string? Units { get; set; }

        [Column("rt_acc_pfs")]
        [StringLength(50)]
        public string? RtAccPfs { get; set; }

        [Column("uncert_fs")]
        [StringLength(50)]
        public string? UncertFs { get; set; }

        [Column("mfg_by")]
        [StringLength(100)]
        public string? MfgBy { get; set; }

        [Column("cal_by")]
        [StringLength(100)]
        public string? CalBy { get; set; }

        [Column("cal_interval")]
        public int? CalInterval { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;

        [ForeignKey("CalId")]
        public virtual CalInfo CalInfo { get; set; } = null!;
    }
}
