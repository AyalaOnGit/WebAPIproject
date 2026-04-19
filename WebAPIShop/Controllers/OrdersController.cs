using Microsoft.AspNetCore.Mvc;
using Services;
using DTOs;
using Repository;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> Get(int id) 
        {
            OrderDTO order = await _orderService.GetOrderById(id);
            if (order != null)
            {
                return Ok(order);
            }
            return NoContent();
        }
  
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Post([FromBody] OrderDTO order)
        {
            OrderDTO createdOrder = await _orderService.AddOrder(order);
            if (createdOrder != null)
                return CreatedAtAction(nameof(Get), new { id = createdOrder.OrderId }, createdOrder);
            return BadRequest("ORDER DONT ACCEPT");
        }
    }
}
