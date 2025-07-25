using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Core.Entities
{
    [Table("cal_info")]
    public class CalInfo
    {
        [Key]
        [Column("cal_id")]
        public Guid CalId { get; set; } = Guid.NewGuid();

        [Column("cal_no")]
        [StringLength(50)]
        public string CalNo { get; set; } = string.Empty;

        [Column("co_id")]
        [StringLength(20)]
        public string? CoId { get; set; }

        [Column("company_id")]
        [StringLength(20)]
        public string? CompanyId { get; set; }

        [Column("order_no")]
        [StringLength(50)]
        public string? OrderNo { get; set; }

        [Column("serial_no")]
        [StringLength(100)]
        public string? SerialNo { get; set; }

        [Column("model_no")]
        [StringLength(100)]
        public string? ModelNumber { get; set; }

        [Column("model_no")]
        [StringLength(100)]
        public string? ModelNo { get; set; }

        [Column("cal_date")]
        public DateTime? CalDate { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column("cal_type")]
        [StringLength(50)]
        public string? CalType { get; set; }

        [Column("cal_tech")]
        [StringLength(50)]
        public string? CalTech { get; set; }

        [Column("tech_id")]
        [StringLength(20)]
        public string? TechId { get; set; }

        [Column("cal_status")]
        [StringLength(20)]
        public string? CalStatus { get; set; }

        [Column("status")]
        [StringLength(20)]
        public string? Status { get; set; }

        [Column("temperature")]
        [Precision(8, 2)]
        public decimal? Temperature { get; set; }

        [Column("humidity")]
        [Precision(5, 2)]
        public decimal? Humidity { get; set; }

        [Column("pressure")]
        [Precision(8, 2)]
        public decimal? Pressure { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;

        [Column("test_by")]
        [StringLength(100)]
        public string? TestBy { get; set; }

        [Column("asrcd_mod_no")]
        [StringLength(100)]
        public string? AsrcdModNo { get; set; }

        [Column("test_type")]
        [StringLength(50)]
        public string? TestType { get; set; }

        public virtual Company? Company { get; set; }
        public virtual ICollection<CalData> CalData { get; set; } = new List<CalData>();
        public virtual ICollection<CalStandards> CalStandards { get; set; } = new List<CalStandards>();
    }
}
