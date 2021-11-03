using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using user_bff.Services;
using user_bff.Models;
using user_bff.Helpers;
using System.Threading.Tasks;

namespace user_bff.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        /// <summary>
        /// Get restaurant list
        /// GET api/<CouponController>
        /// </summary>
        /// <returns>Restaurant list</returns>
        [HttpGet]
        public ActionResult<List<Restaurant>> GetAll()
        {
            try
            {
                var restro = _restaurantService.GetAll();

                return Ok(new
                {
                    data = restro,
                    message = "Successfully returned restaurant list."
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get search result from restaurants
        /// GET api/<CouponController>/search/text
        /// </summary>
        /// <param name="SearchText">Search Text</param>
        /// <returns>Search list</returns>
        [HttpGet("{searchText}")]
        public ActionResult<List<Restaurant>> GetSearch(string SearchText)
        {
            try
            {
                var restro = _restaurantService.GetSearch(SearchText);

                return Ok(new
                {
                    data = restro,
                    message = "Successfully returned search list."
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get restaurant detail
        /// GET api/<CouponController>/5
        /// </summary>
        /// <param name="id">Restaurant id</param>
        /// <returns>Restaurant Entity</returns>
        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> GetById(Guid restaurantId)
        {
            try
            {
                var restro = _restaurantService.GetById(restaurantId);

                return Ok(new
                {
                    data = restro,
                    message = "Successfully returned restaurant details."
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