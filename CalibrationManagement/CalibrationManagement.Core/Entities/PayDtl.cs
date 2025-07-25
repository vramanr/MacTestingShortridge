using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Core.Entities
{
    [Table("pay_dtl")]
    public class PayDtl
    {
        [Key]
        [Column("payment_id")]
        public Guid PaymentId { get; set; } = Guid.NewGuid();

        [Column("invoice_no")]
        [StringLength(50)]
        public string? InvoiceNo { get; set; }

        [Column("payment_date")]
        public DateTime? PaymentDate { get; set; }

        [Column("payment_amount")]
        [Precision(12, 2)]
        public decimal? PaymentAmount { get; set; }

        [Column("payment_method")]
        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        [Column("check_no")]
        [StringLength(50)]
        public string? CheckNo { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;

        public virtual Invoice? Invoice { get; set; }
    }
}
