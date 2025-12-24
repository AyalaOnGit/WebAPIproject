using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Services;
using DTOs;
using Repository;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIShop.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;
        
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> Get(int id) 
        {
            OrderDTO order = await _orderService.GetOrderById(id);
            if(order!= null)
            {
                return Ok(order);
            }
            return NoContent();
        }
  
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Post([FromBody] OrderDTO order)
        {
            OrderDTO createdOrder = await _orderService.AddOrder(order);
            if(createdOrder != null)
                return CreatedAtAction(nameof(Get), new{id = createdOrder.OrderId}, createdOrder);
            return BadRequest("ORDER DONT ACCEPT");
        }

    }
}
