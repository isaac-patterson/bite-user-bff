using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_bff.Helpers;
using user_bff.Models;
using user_bff.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace user_bff.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// POST: Charge/Take online payment 
        /// </summary>
        /// <param name="payModel">paymodel</param>
        /// <returns>object</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Charge([FromBody] PayModel payModel)
        {
            try
            {
                var charge = _paymentService.TakePayment(payModel);
                if (charge.Paid)
                {
                    return Ok(new
                    {
                        data = charge.Id,
                        message = "Payment completed successfully."
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        error = string.Format("Payment failed: {0}.", charge.FailureMessage)
                    });
                }
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }

       
        /// <summary>
        /// Get: Refund payment
        /// </summary>
        /// <param name="chargeId">chargeId</param>
        /// <returns>Object</returns>
        [HttpGet("{chargeId}")]
        [Authorize]
        public IActionResult Refund(string chargeId)
        {
            try
            {
                var refund = _paymentService.Refund(chargeId);
                if (refund.Status == "succeeded")
                {
                    return Ok(new
                    {
                        message = "Payment refund completed successfully."
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        error = string.Format("Refund failed: {0}.", refund.FailureReason)
                    });
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
