using AutoMapper;
using eCommerceOrders.FinishPurchase.Dtos;
using eCommerceOrders.FinishPurchase.Models;
using eCommerceOrders.FinishPurchase.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace eCommerceOrders.FinishPurchase.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResult>>> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var ordersResult = _mapper.Map<List<OrderResult>>(orders);

            return Ok(ordersResult);
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<ActionResult<OrderResult>> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            var orderResult = _mapper.Map<OrderResult>(order);

            if (orderResult == null)
            {
                return NotFound();
            }

            return Ok(orderResult);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderPost orderPost)
        {
            var order = _mapper.Map<Order>(orderPost);
            var createdOrder = await _orderService.CreateOrderAsync(order);

            /* Configuração de serialização para lidar com ciclos de referência */
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            };

            string jsonString = JsonSerializer.Serialize(createdOrder, options);

            return CreatedAtAction("GetById", new { id = createdOrder.Id }, jsonString);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderPost orderPost)
        {
            if (id != orderPost.Id)
            {
                return BadRequest();
            }

            var order = _mapper.Map<Order>(orderPost);
            var updatedOrder = await _orderService.UpdateOrderAsync(order);

            if (updatedOrder == null)
            {
                return NotFound();
            }

            /* Configuração de serialização para lidar com ciclos de referência */
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            };

            string jsonString = JsonSerializer.Serialize(updatedOrder, options);

            return Ok(jsonString);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _orderService.DeleteOrderAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}