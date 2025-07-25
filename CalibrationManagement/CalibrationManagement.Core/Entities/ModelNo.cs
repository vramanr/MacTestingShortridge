using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalibrationManagement.Core.Entities
{
    [Table("model_no")]
    public class ModelNo
    {
        [Key]
        [Column("model_id")]
        public Guid ModelId { get; set; } = Guid.NewGuid();

        [Column("model_no_id")]
        public Guid ModelNoId { get; set; } = Guid.NewGuid();

        [Column("model_no")]
        [StringLength(100)]
        public string? ModelNumber { get; set; }

        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Column("cal_interval")]
        public int? CalInterval { get; set; }

        [Column("active")]
        public bool? Active { get; set; }

        [Column("serial_no")]
        [StringLength(100)]
        public string SerialNo { get; set; } = string.Empty;

        [Column("co_id")]
        [StringLength(20)]
        public string? CoId { get; set; }

        [Column("co_name")]
        [StringLength(200)]
        public string? CoName { get; set; }

        [Column("order_no")]
        [StringLength(50)]
        public string? OrderNo { get; set; }

        [Column("cal_date")]
        public DateTime? CalDate { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column("cal_type")]
        [StringLength(50)]
        public string? CalType { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string? Status { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;

        public virtual Company? Company { get; set; }
    }
}
