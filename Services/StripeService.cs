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
        /// <param name="payModel">Payment model</param>
        /// <returns>Object</returns>
        Stripe.Charge Charge(string customerId, PayModel payModel);

        /// <summary>
        /// Use: Refund stripe paymennt
        /// </summary>
        /// <param name="chargeId">chargeId</param>
        /// <returns>Object</returns>
        Stripe.Refund Refund(string chargeId);

        /// <summary>
        /// Use: Get Charge
        /// </summary>
        /// <param name="chargeId"></param>
        /// <returns></returns>
        Stripe.Charge GetCharge(string chargeId);

        /// <summary>
        ///Use: Update Charge
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Stripe.Charge UpdateCharge(string chargeId, string refundId);

        /// <summary>
        /// Use: Create Payment Intent on stripe
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Stripe.PaymentIntent CreatePaymentIntent(long amount, string currency, string customerId = null);


        /// <summary>
        /// Use: Get stripe token
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        Token GetStripeToken(string tokenId);

        /// <summary>
        /// Use: create stripe token
        /// </summary>
        /// <param name="payModel"></param>
        /// <returns></returns>
        Token CreateStripeToken(Models.Card card);

        /// <summary>
        /// Use: create stripe customer
        /// </summary>
        /// <param name="payModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Customer CreateStripeCustomer(PayModel payModel, Token token);

        /// <summary>
        /// Use: Get stripe customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Customer GetStripeCustomer(string customerId);

    }

    public class StripeService : IStripeService
    {
        private readonly IConfiguration _config;
        public readonly string _ApiKey;
        public readonly string _currency;
        //private readonly ICardTokenService _CardTokenService;

        public StripeService(IConfiguration config)
        {
            _config = config;
            _ApiKey = _config.GetValue<string>("Stripe:Secretkey");
            _currency = _config.GetValue<string>("Stripe:Currency");
        }

        ///<inheritdoc/>
        public Stripe.Charge Charge(string customerId, PayModel payModel)
        {
            try
            {
                StripeConfiguration.ApiKey = _ApiKey;
                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = payModel.Amount,
                    Currency = _currency,
                    Description = string.Format("Payment for order {0}", payModel.OrderId),
                    //Source = token.Id,
                    Metadata = new Dictionary<string, string>
                      {
                        { "order_id", payModel.OrderId.ToString() },
                      }
                };
                if (!string.IsNullOrEmpty(customerId))
                {
                    chargeOptions.Customer = customerId;
                }
                else
                {
                    var token = CreateStripeToken(payModel);
                    chargeOptions.Source = token.Id;
                }

                var chargeService = new ChargeService();
                return chargeService.Create(chargeOptions);
            }
            catch (StripeException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public Stripe.Refund Refund(string chargeId)
        {
            try
            {
                StripeConfiguration.ApiKey = _ApiKey;
                var options = new RefundCreateOptions
                {
                    Charge = chargeId,
                };

                var service = new RefundService();
                return service.Create(options);
            }
            catch (StripeException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public Token CreateStripeToken(Models.Card card)
        {
            try
            {
                StripeConfiguration.ApiKey = _ApiKey;
                var options = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = card.CardNumder,
                        ExpMonth = card.Month,
                        ExpYear = card.Year,
                        Cvc = card.CVC
                    },
                };

                var serviceToken = new TokenService();
                Token stripeToken = serviceToken.Create(options);
                return stripeToken;
            }
            catch (StripeException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public Token GetStripeToken(string tokenId)
        {
            try
            {
                StripeConfiguration.ApiKey = _ApiKey;
                var service = new TokenService();
                return service.Get(tokenId);
            }
            catch (StripeException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public Stripe.Charge GetCharge(string chargeId)
        {
            try
            {
                StripeConfiguration.ApiKey = _ApiKey;
                var service = new ChargeService();
                return service.Get(chargeId);
            }
            catch (StripeException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public Stripe.Charge UpdateCharge(string chargeId, string refundId)
        {
            try
            {
                StripeConfiguration.ApiKey = _ApiKey;
                var options = new ChargeUpdateOptions
                {
                    Metadata = new Dictionary<string, string>
                  {
                    { "Refund_Id",refundId},
                  },
                };
                var service = new ChargeService();
                return service.Update(
                  chargeId,
                  options
                );
            }
            catch (StripeException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public Stripe.PaymentIntent CreatePaymentIntent(long amount, string currency, string customerId)
        {
            try
            {
                StripeConfiguration.ApiKey = _ApiKey;

                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = currency,
                    PaymentMethodTypes = new List<string>
                  {
                    "card",
                  },
                };
                var service = new PaymentIntentService();
                return service.Create(options);
            }
            catch (StripeException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public Customer CreateStripeCustomer(PayModel payModel, Token token)
        {
            StripeConfiguration.ApiKey = _ApiKey;
            var customerOptions = new CustomerCreateOptions
            {
                Email = payModel.EmailId,
                Source = token.Id
            };
            var customerService = new CustomerService();
            return customerService.Create(customerOptions);
        }

        ///<inheritdoc/>
        public Customer GetStripeCustomer(string customerId)
        {
            StripeConfiguration.ApiKey = _ApiKey;
            var service = new CustomerService();
            return service.Get(customerId);
        }
    }
}
