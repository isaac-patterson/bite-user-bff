using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_bff.Helpers;
using user_bff.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace user_bff.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
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
        /// <param name="db">Database context service</param>
        /// <param name="couponCode">User requested discount coupon code</param>
        /// <returns></returns>
        [HttpGet("{couponCode}")]
        public IActionResult Get(string couponCode)
        {
            try
            {
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
