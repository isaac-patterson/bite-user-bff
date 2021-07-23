using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_bff.Helpers;
using user_bff.Models;

namespace user_bff.Services
{
    public interface IStripeService
    {
        /// <summary>
        /// Use: Take payment from stripe
        /// </summary>
        /// <param name="chargeCreateOptions">Charge option</param>
        /// <returns>Object</returns>
        Charge Charge(PayModel payModel);

        /// <summary>
        /// Use: Refund stripe paymennt
        /// </summary>
        /// <param name="refundCreateOptions">Refund create option</param>
        /// <returns>Object</returns>
        Refund Refund(string chargeId);
    }

    public class StripeService : IStripeService
    {
        private readonly IConfiguration _config;
        public readonly string _ApiKey;
        public readonly string _currency;

        public StripeService(IConfiguration config)
        {
            _config = config;
            _ApiKey = _config.GetValue<string>("Stripe:Secretkey");
            _currency = _config.GetValue<string>("Stripe:Currency");
        }

        ///<inheritdoc/>
        public Charge Charge(PayModel payModel)
        {
            try
            {
                StripeConfiguration.ApiKey = _ApiKey;
                Token stripeToken = CreateStripeToken(payModel);

                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = payModel.Amount,
                    Currency = _currency,
                    Description = string.Format("Payment for order {0}", payModel.OrderId),
                    Source = stripeToken.Id
                };

                var chargeService = new ChargeService();
                return chargeService.Create(chargeOptions);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public Refund Refund(string chargeId)
        {
            try {
                StripeConfiguration.ApiKey = _ApiKey;
                var options = new RefundCreateOptions
                {
                    Charge = chargeId,
                };

                var service = new RefundService();
                return service.Create(options);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        private static Token CreateStripeToken(PayModel payModel)
        {
            try {
                var options = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = payModel.CardNumder,
                        ExpMonth = payModel.Month,
                        ExpYear = payModel.Year,
                        Cvc = payModel.CVC
                    },
                };

                var serviceToken = new TokenService();
                Token stripeToken = serviceToken.Create(options);
                return stripeToken;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }
    }
}
