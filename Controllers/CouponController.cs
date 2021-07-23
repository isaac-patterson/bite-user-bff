using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_bff.Helpers;
using user_bff.Models;
using user_bff.Services;

namespace user_bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        /// <summary>
        /// Get: Validate discount coupon code
        /// </summary>
        /// <param name="couponCode">User requested discount coupon code</param>
        /// <returns></returns>
        [HttpGet("{couponCode}")]
        public IActionResult Get(string couponCode)
        {
            try
            {
                //if (!_couponService.IsCouponExists(couponCode))
                //{
                //    return NotFound(string.Format("Coupon not found"));
                //}
                var coupon = _couponService.GetById(couponCode);

                return Ok(new
                {
                    data = coupon,
                    message = "Successfully returned coupon detail."
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get: Validate discount coupon code
        /// </summary>
        /// <param name="couponCode">User requested discount coupon code</param>
        /// <returns></returns>
        [HttpGet("/validate/{couponCode}")]
        public IActionResult Validate(string couponCode)
        {
            try
            {
                if (_couponService.IsCouponExists(couponCode))
                {
                    return NotFound(string.Format("Valid"));
                }
                else {
                    return NotFound(string.Format("Invalid"));
                }
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }
    }

}
