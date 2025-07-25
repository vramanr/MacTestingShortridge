using CalibrationManagement.Application.Services;
using CalibrationManagement.Application.DTOs;
using CalibrationManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace CalibrationManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        [HttpGet("by-number/{orderNo}")]
        public async Task<ActionResult<OrderDto>> GetOrderByNumber(string orderNo)
        {
            var order = await _orderService.GetOrderByNumberAsync(orderNo);
            if (order == null)
                return NotFound();

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        [HttpGet("by-company/{coId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByCompany(string coId)
        {
            var orders = await _orderService.GetOrdersByCompanyAsync(coId);
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        [HttpGet("by-status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByStatus(string status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        [HttpGet("by-date-range")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = _mapper.Map<OrdrStat>(createOrderDto);
            var createdOrder = await _orderService.CreateOrderAsync(order);
            var createdOrderDto = _mapper.Map<OrderDto>(createdOrder);
            
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.OrderId }, createdOrderDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> UpdateOrder(Guid id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            if (id != updateOrderDto.OrderId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingOrder = await _orderService.GetOrderByIdAsync(id);
            if (existingOrder == null)
                return NotFound();

            var order = _mapper.Map<OrdrStat>(updateOrderDto);
            var updatedOrder = await _orderService.UpdateOrderAsync(order);
            var updatedOrderDto = _mapper.Map<OrderDto>(updatedOrder);
            
            return Ok(updatedOrderDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{orderNo}/details")]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetOrderDetails(string orderNo)
        {
            var orderDetails = await _orderService.GetOrderDetailsAsync(orderNo);
            var orderDetailDtos = _mapper.Map<IEnumerable<OrderDetailDto>>(orderDetails);
            return Ok(orderDetailDtos);
        }

        [HttpPost("details")]
        public async Task<ActionResult<OrderDetailDto>> AddOrderDetail([FromBody] CreateOrderDetailDto createOrderDetailDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderDetail = _mapper.Map<OrDetail>(createOrderDetailDto);
            var createdDetail = await _orderService.AddOrderDetailAsync(orderDetail);
            var createdDetailDto = _mapper.Map<OrderDetailDto>(createdDetail);
            
            return CreatedAtAction(nameof(GetOrderDetails), new { orderNo = orderDetail.OrderNo }, createdDetailDto);
        }

        [HttpPost("generate-number")]
        public async Task<ActionResult<string>> GenerateOrderNumber()
        {
            var orderNumber = await _orderService.GenerateOrderNumberAsync();
            return Ok(new { OrderNumber = orderNumber });
        }
    }
}
