using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Core.Entities
{
    [Table("ordrstat")]
    public class OrdrStat
    {
        [Key]
        [Column("order_id")]
        public Guid OrderId { get; set; } = Guid.NewGuid();

        [Column("order_no")]
        [StringLength(50)]
        public string OrderNo { get; set; } = string.Empty;

        [Column("co_id")]
        [StringLength(20)]
        public string? CoId { get; set; }

        [Column("order_date")]
        public DateTime? OrderDate { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string? Status { get; set; }

        [Column("priority")]
        [StringLength(20)]
        public string? Priority { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("total_amount")]
        [Precision(12, 2)]
        public decimal? TotalAmount { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;

        public virtual Company? Company { get; set; }
        public virtual ICollection<OrDetail> OrderDetails { get; set; } = new List<OrDetail>();
    }
}
