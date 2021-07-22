using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using user_bff.Models;
using user_bff.Services;
using user_bff.Helpers;
using Newtonsoft.Json;

namespace user_bff.Controllers
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

        /// <summary>
        /// Get order by order id
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>Order entity</returns>
        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id)
        {
            try
            {
                var order = _orderService.GetById(id);
                return Ok(new
                {
                    data = order,
                    message = "Successfully returned order detail."
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Create new order into system
        /// </summary>
        /// <param name="order">Order entity</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create([FromBody] Order order)
        {
            try
            {
                var _order = _orderService.GetById(order.OrderId);
                return Ok(new
                {
                    data = order,
                    message = "Successfully create a new pending order."
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
