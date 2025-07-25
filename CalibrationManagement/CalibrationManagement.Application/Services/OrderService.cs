using CalibrationManagement.Core.Entities;
using CalibrationManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Application.Services
{
    public interface IOrderService
    {
        Task<OrdrStat> CreateOrderAsync(OrdrStat order);
        Task<OrdrStat> UpdateOrderAsync(OrdrStat order);
        Task<OrdrStat?> GetOrderByIdAsync(Guid orderId);
        Task<OrdrStat?> GetOrderByNumberAsync(string orderNo);
        Task<IEnumerable<OrdrStat>> GetOrdersByCompanyAsync(string coId);
        Task<IEnumerable<OrdrStat>> GetOrdersByStatusAsync(string status);
        Task<IEnumerable<OrdrStat>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> DeleteOrderAsync(Guid orderId);
        Task<OrDetail> AddOrderDetailAsync(OrDetail orderDetail);
        Task<IEnumerable<OrDetail>> GetOrderDetailsAsync(string orderNo);
        Task<string> GenerateOrderNumberAsync();
    }

    public class OrderService : IOrderService
    {
        private readonly CalibrationDbContext _context;

        public OrderService(CalibrationDbContext context)
        {
            _context = context;
        }

        public async Task<OrdrStat> CreateOrderAsync(OrdrStat order)
        {
            if (string.IsNullOrEmpty(order.OrderNo))
            {
                order.OrderNo = await GenerateOrderNumberAsync();
            }

            order.CreatedDate = DateTime.UtcNow;

            _context.OrdrStats.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<OrdrStat> UpdateOrderAsync(OrdrStat order)
        {
            _context.OrdrStats.Update(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<OrdrStat?> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.OrdrStats
                .Include(o => o.Company)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && !o.Deleted);
        }

        public async Task<OrdrStat?> GetOrderByNumberAsync(string orderNo)
        {
            return await _context.OrdrStats
                .Include(o => o.Company)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderNo == orderNo && !o.Deleted);
        }

        public async Task<IEnumerable<OrdrStat>> GetOrdersByCompanyAsync(string coId)
        {
            return await _context.OrdrStats
                .Include(o => o.Company)
                .Where(o => o.CoId == coId && !o.Deleted)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrdrStat>> GetOrdersByStatusAsync(string status)
        {
            return await _context.OrdrStats
                .Include(o => o.Company)
                .Where(o => o.Status == status && !o.Deleted)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrdrStat>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.OrdrStats
                .Include(o => o.Company)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && !o.Deleted)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            var order = await _context.OrdrStats.FindAsync(orderId);
            if (order == null)
                return false;

            order.Deleted = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OrDetail> AddOrderDetailAsync(OrDetail orderDetail)
        {
            orderDetail.CreatedDate = DateTime.UtcNow;
            
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            return orderDetail;
        }

        public async Task<IEnumerable<OrDetail>> GetOrderDetailsAsync(string orderNo)
        {
            return await _context.OrderDetails
                .Where(od => od.OrderNo == orderNo && !od.Deleted)
                .OrderBy(od => od.LineNo)
                .ToListAsync();
        }

        public async Task<string> GenerateOrderNumberAsync()
        {
            var year = DateTime.Now.Year.ToString();
            var lastOrder = await _context.OrdrStats
                .Where(o => o.OrderNo.StartsWith(year))
                .OrderByDescending(o => o.OrderNo)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastOrder != null && lastOrder.OrderNo.Length > 4)
            {
                var numberPart = lastOrder.OrderNo.Substring(4);
                if (int.TryParse(numberPart, out var lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{year}{nextNumber:D4}";
        }
    }
}
