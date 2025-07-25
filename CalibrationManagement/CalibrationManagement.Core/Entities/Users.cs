using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalibrationManagement.Core.Entities
{
    [Table("users")]
    public class Users
    {
        [Key]
        [Column("user_id")]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Column("password_hash")]
        [StringLength(255)]
        public string? PasswordHash { get; set; }

        [Column("first_name")]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Column("last_name")]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Column("role")]
        [StringLength(50)]
        public string? Role { get; set; }

        [Column("active")]
        public bool Active { get; set; } = true;

        [Column("last_login")]
        public DateTime? LastLogin { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;
    }
}
