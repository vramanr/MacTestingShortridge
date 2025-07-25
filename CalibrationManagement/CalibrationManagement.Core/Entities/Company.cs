using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Core.Entities
{
    [Table("company")]
    public class Company
    {
        [Key]
        [Column("company_id")]
        public Guid CompanyId { get; set; } = Guid.NewGuid();

        [Column("co_id")]
        [StringLength(20)]
        public string CoId { get; set; } = string.Empty;

        [Column("co_name")]
        [StringLength(200)]
        public string? CoName { get; set; }

        [Column("name")]
        [StringLength(200)]
        public string? Name { get; set; }

        [Column("address1")]
        [StringLength(100)]
        public string? Address1 { get; set; }

        [Column("address")]
        [StringLength(100)]
        public string? Address { get; set; }

        [Column("address2")]
        [StringLength(100)]
        public string? Address2 { get; set; }

        [Column("city")]
        [StringLength(50)]
        public string? City { get; set; }

        [Column("state")]
        [StringLength(10)]
        public string? State { get; set; }

        [Column("zip")]
        [StringLength(20)]
        public string? Zip { get; set; }

        [Column("zip_code")]
        [StringLength(20)]
        public string? ZipCode { get; set; }

        [Column("tax_rate")]
        [Precision(5, 4)]
        public decimal? TaxRate { get; set; }

        [Column("discount_rate")]
        [Precision(5, 4)]
        public decimal? DiscountRate { get; set; }

        [Column("country")]
        [StringLength(50)]
        public string? Country { get; set; }

        [Column("phone")]
        [StringLength(30)]
        public string? Phone { get; set; }

        [Column("fax")]
        [StringLength(30)]
        public string? Fax { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Column("website")]
        [StringLength(200)]
        public string? Website { get; set; }

        [Column("tax_id")]
        [StringLength(50)]
        public string? TaxId { get; set; }

        [Column("terms")]
        [StringLength(50)]
        public string? Terms { get; set; }

        [Column("discount_code")]
        [StringLength(20)]
        public string? DiscountCode { get; set; }

        [Column("credit_limit")]
        [Precision(12, 2)]
        public decimal? CreditLimit { get; set; }

        [Column("balance")]
        [Precision(12, 2)]
        public decimal? Balance { get; set; }

        [Column("active")]
        public bool Active { get; set; } = true;

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;

        [Column("s_city")]
        [StringLength(100)]
        public string? SCity { get; set; }

        [Column("s_state")]
        [StringLength(50)]
        public string? SState { get; set; }

        public virtual ICollection<CalInfo> CalInfos { get; set; } = new List<CalInfo>();
        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public virtual ICollection<OrdrStat> OrdrStats { get; set; } = new List<OrdrStat>();
        public virtual ICollection<ModelNo> ModelNumbers { get; set; } = new List<ModelNo>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
