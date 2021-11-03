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
    [Authorize]
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
        [HttpGet("{orderId}")]
        public IActionResult Refund(int orderId)
        {
            try
            {
                var refund = _paymentService.Refund(orderId);
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

        /// <summary>
        /// POST: Create Payment Intent 
        /// </summary>
        /// <param name="amount">amount</param>
        /// <param name="currency">currency</param>
        /// <param name="customerId">customerId</param>
        /// <returns>object</returns>
        [HttpPost]
        public IActionResult CreatePaymentIntent(long amount, string currency, string customerId)
        {
            try
            {
                var intent = _paymentService.CreatePaymentIntent(amount, currency, customerId);
                return Ok(new
                {
                    data = intent,
                    message = "Payment intent created successfully."
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
