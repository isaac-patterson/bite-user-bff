using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_bff.Models;

namespace user_bff.Services
{
    public interface IPaymentService
    {
        /// <summary>
        /// Use: Take online payment
        /// </summary>
        /// <param name="payModel">pay model</param>
        /// <returns>object</returns>
        Charge TakePayment(PayModel payModel);

        /// <summary>
        /// Use: Refund payment by charge Id
        /// </summary>
        /// <param name="chargeId">charge Id</param>
        /// <returns>Object</returns>
        Refund Refund(string chargeId);
    }

    public class PaymentService : IPaymentService
    {
        private IStripeService _stripeService;
        public PaymentService(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        ///<Inheritdoc/>
        public Refund Refund(string chargeId)
        {
            try
            {
                return _stripeService.Refund(chargeId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        ///<Inheritdoc/>
        public Charge TakePayment(PayModel payModel)
        {
            try
            {
                return _stripeService.Charge(payModel);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
