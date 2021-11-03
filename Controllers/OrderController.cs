using Microsoft.AspNetCore.Mvc;
using user_bff.Models;
using user_bff.Services;
using user_bff.Helpers;
using Microsoft.AspNetCore.Authorization;
using System;

namespace user_bff.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get order history
        /// GET api/<OrderController>/history/5
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Order list</returns>
        [HttpGet("{CognitoUserId}")]
        public ActionResult<Order> GetAll(Guid CognitoUserId)
        {
            try
            {
                var order = _orderService.GetAll(CognitoUserId);
                return Ok(new
                {
                    data = order,
                    message = "Successfully returned order list."
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get order by order id
        /// GET api/<OrderController>/5
        /// </summary>
        /// <param name="db">Database context service</param>
        /// <param name="id">Order id</param>
        /// <returns>Order entity</returns>
        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public ActionResult<Order> GetById(int id)
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
        /// POST api/<OrderController>
        /// </summary>
        /// <param name="db">Database context service</param>
        /// <param name="order">Order entity</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create([FromBody] Order order)
        {
            try
            {
                var _order = _orderService.Create(order);
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

        /// <summary>
        /// Get order status by order id
        /// GET api/<OrderController>/5
        /// </summary>
        /// <param name="db">Database context service</param>
        /// <param name="id">Order id</param>
        /// <returns>Order entity</returns>
        // GET api/<OrderController>/5
        [HttpGet("status/{id}")]
        public ActionResult<Order> GetStatusById(int id)
        {
            try
            {
                var order = _orderService.GetStatusById(id);
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

    }
}
