using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_bff.Models;
using user_bff.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace user_bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        public CouponController() { }

        /// <summary>
        /// Validate discount coupon code
        /// GET api/<CouponController>/xyz123
        /// </summary>
        /// <param name="db">Database context service</param>
        /// <param name="couponCode">User requested discount coupon code</param>
        /// <returns></returns>
        [HttpGet("{couponCode}")]
        public IActionResult Get([FromServices] DBContext db, string couponCode)
        {
            if (!db.Coupon.Any(x => x.DiscountCode == couponCode))
            {
                return NotFound(string.Format("Invalid Coupon: Coupon not found"));
            }
            else if (db.Coupon.Any(x => x.DiscountCode == couponCode && x.ExpiryDate > DateTime.UtcNow))
            {
                return BadRequest(string.Format("Invalid Coupon: Coupon code is exipred"));
            }
            return Ok(db.Coupon.FirstOrDefault(x => x.DiscountCode == couponCode));
        }
    }
}
