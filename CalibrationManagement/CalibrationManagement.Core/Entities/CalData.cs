using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Core.Entities
{
    [Table("cal_data")]
    public class CalData
    {
        [Key]
        [Column("cal_data_id")]
        public Guid CalDataId { get; set; } = Guid.NewGuid();

        [Column("cal_id")]
        public Guid CalId { get; set; }

        [Column("cal_no")]
        [StringLength(50)]
        public string? CalNo { get; set; }

        [Column("mode")]
        [StringLength(50)]
        public string? Mode { get; set; }

        [Column("set_point")]
        [Precision(15, 6)]
        public decimal? SetPoint { get; set; }

        [Column("actual_reading")]
        [Precision(15, 6)]
        public decimal? ActualReading { get; set; }

        [Column("deviation")]
        [Precision(15, 6)]
        public decimal? Deviation { get; set; }

        [Column("percent_deviation")]
        [Precision(8, 4)]
        public decimal? PercentDeviation { get; set; }

        [Column("tolerance")]
        [Precision(15, 6)]
        public decimal? Tolerance { get; set; }

        [Column("pass_fail")]
        [StringLength(10)]
        public string? PassFail { get; set; }

        [Column("units")]
        [StringLength(20)]
        public string? Units { get; set; }

        [Column("sequence_no")]
        public int? SequenceNo { get; set; }

        [Column("point_no")]
        public int? PointNo { get; set; }

        [Column("nominal_value")]
        [Precision(15, 6)]
        public decimal? NominalValue { get; set; }

        [Column("as_found_value")]
        [Precision(15, 6)]
        public decimal? AsFoundValue { get; set; }

        [Column("as_left_value")]
        [Precision(15, 6)]
        public decimal? AsLeftValue { get; set; }

        [Column("tolerance_value")]
        [Precision(15, 6)]
        public decimal? ToleranceValue { get; set; }

        [Column("reading")]
        [Precision(15, 6)]
        public decimal? Reading { get; set; }

        [Column("status")]
        [StringLength(20)]
        public string? Status { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;

        [Column("report_tol")]
        [StringLength(50)]
        public string? ReportTol { get; set; }

        [Column("std_read")]
        [Precision(15, 6)]
        public decimal? StdRead { get; set; }

        [Column("test_read")]
        [Precision(15, 6)]
        public decimal? TestRead { get; set; }

        [Column("allow_dev")]
        [Precision(15, 6)]
        public decimal? AllowDev { get; set; }

        [Column("actual_dev")]
        [Precision(15, 6)]
        public decimal? ActualDev { get; set; }

        [ForeignKey("CalId")]
        public virtual CalInfo CalInfo { get; set; } = null!;
    }
}
