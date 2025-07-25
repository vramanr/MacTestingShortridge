using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Core.Entities
{
    [Table("invoice")]
    public class Invoice
    {
        [Key]
        [Column("invoice_id")]
        public Guid InvoiceId { get; set; } = Guid.NewGuid();

        [Column("invoice_no")]
        [StringLength(50)]
        public string InvoiceNo { get; set; } = string.Empty;

        [Column("co_id")]
        [StringLength(20)]
        public string? CoId { get; set; }

        [Column("order_no")]
        [StringLength(50)]
        public string? OrderNo { get; set; }

        [Column("inv_date")]
        public DateTime? InvDate { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column("subtotal")]
        [Precision(12, 2)]
        public decimal? Subtotal { get; set; }

        [Column("tax_amount")]
        [Precision(12, 2)]
        public decimal? TaxAmount { get; set; }

        [Column("total_amount")]
        [Precision(12, 2)]
        public decimal? TotalAmount { get; set; }

        [Column("paid_amount")]
        [Precision(12, 2)]
        public decimal? PaidAmount { get; set; }

        [Column("balance")]
        [Precision(12, 2)]
        public decimal? Balance { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string? Status { get; set; }

        [Column("qa_code")]
        [StringLength(20)]
        public string? QaCode { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;

        public virtual Company? Company { get; set; }
        public virtual ICollection<PayDtl> PaymentDetails { get; set; } = new List<PayDtl>();
    }
}
