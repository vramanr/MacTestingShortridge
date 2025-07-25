using System.ComponentModel.DataAnnotations;

namespace CalibrationManagement.Application.DTOs
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string OrderNo { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? CoId { get; set; }
        
        public DateTime? OrderDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [StringLength(20)]
        public string? Status { get; set; }
        
        [StringLength(50)]
        public string? Priority { get; set; }
        
        public decimal? TotalAmount { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public CompanyDto? Company { get; set; }
        
        public List<OrderDetailDto> OrderDetails { get; set; } = new();
    }

    public class CreateOrderDto
    {
        [StringLength(20)]
        public string? CoId { get; set; }
        
        public DateTime? OrderDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "PENDING";
        
        [StringLength(50)]
        public string? Priority { get; set; }
        
        public decimal? TotalAmount { get; set; }
        
        public string? Notes { get; set; }
    }

    public class UpdateOrderDto
    {
        [Required]
        public Guid OrderId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string OrderNo { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? CoId { get; set; }
        
        public DateTime? OrderDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [StringLength(20)]
        public string? Status { get; set; }
        
        [StringLength(50)]
        public string? Priority { get; set; }
        
        public decimal? TotalAmount { get; set; }
        
        public string? Notes { get; set; }
    }

    public class OrderDetailDto
    {
        public Guid OrderDetailId { get; set; }
        
        public string? OrderNo { get; set; }
        
        public int? LineNo { get; set; }
        
        public string? ItemDescription { get; set; }
        
        public int? Quantity { get; set; }
        
        public decimal? UnitPrice { get; set; }
        
        public decimal? TotalPrice { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }

    public class CreateOrderDetailDto
    {
        [Required]
        [StringLength(50)]
        public string OrderNo { get; set; } = string.Empty;
        
        public int? LineNo { get; set; }
        
        [Required]
        [StringLength(200)]
        public string ItemDescription { get; set; } = string.Empty;
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal UnitPrice { get; set; }
    }
}
