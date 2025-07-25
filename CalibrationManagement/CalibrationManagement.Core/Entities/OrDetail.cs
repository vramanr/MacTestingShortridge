using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Core.Entities
{
    [Table("ordetail")]
    public class OrDetail
    {
        [Key]
        [Column("order_detail_id")]
        public Guid OrderDetailId { get; set; } = Guid.NewGuid();

        [Column("serial_no")]
        [StringLength(100)]
        public string? SerialNo { get; set; }

        [Column("model_no")]
        [StringLength(100)]
        public string? ModelNumber { get; set; }

        [Column("total_price")]
        [Precision(12, 2)]
        public decimal? TotalPrice { get; set; }

        [Column("order_no")]
        [StringLength(50)]
        public string? OrderNo { get; set; }

        [Column("line_no")]
        public int? LineNo { get; set; }

        [Column("item_description")]
        [StringLength(500)]
        public string? ItemDescription { get; set; }

        [Column("part_no")]
        [StringLength(100)]
        public string? PartNo { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("quantity")]
        public int? Quantity { get; set; }

        [Column("unit_price")]
        [Precision(12, 2)]
        public decimal? UnitPrice { get; set; }

        [Column("extended_price")]
        [Precision(12, 2)]
        public decimal? ExtendedPrice { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;

        public virtual OrdrStat? Order { get; set; }
    }
}
