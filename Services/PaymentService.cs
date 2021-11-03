using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using user_bff.Helpers;
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
        Stripe.Charge TakePayment(PayModel payModel);

        /// <summary>
        /// Use: Refund payment by charge Id
        /// </summary>
        /// <param name="orderId">charge Id</param>
        /// <returns>Object</returns>
        Stripe.Refund Refund(int orderId);

        /// <summary>
        /// Use: Create Payment Intent on stripe
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Stripe.PaymentIntent CreatePaymentIntent(long amount, string currency, string customerId);
    }

    public class PaymentService : IPaymentService
    {
        private IStripeService _stripeService;
        private IOrderService _orderService;
        private DBContext _context;
        private ICardTokenService _CardTokenService;

        public PaymentService(IStripeService stripeService, DBContext context, IOrderService orderService, ICardTokenService CardTokenService)
        {
            _stripeService = stripeService;
            _context = context;
            _orderService = orderService;
            _CardTokenService = CardTokenService;
        }

        ///<Inheritdoc/>
        public Stripe.PaymentIntent CreatePaymentIntent(long amount, string currency, string customerId)
        {
            return _stripeService.CreatePaymentIntent(amount, currency, customerId);
        }

        ///<Inheritdoc/>
        public Stripe.Refund Refund(int orderId)
        {
            Stripe.Refund refund = null;
            try
            {
                var charge = GetCharge(orderId);
                if (charge.Refunded == true)
                {
                    throw new AppException("No refund available for this order.");
                }

                refund = _stripeService.Refund(charge.Id);
                SaveRefund(orderId, refund);
                var updatedStripeCharge = _stripeService.UpdateCharge(charge.Id, refund.Id);
                UpdateCharge(orderId, updatedStripeCharge);

            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
            return refund;
        }

        ///<Inheritdoc/>
        public Stripe.Charge TakePayment(PayModel payModel)
        {
            Stripe.Charge charge = null;
            try
            {
                var order = _orderService.GetById(payModel.OrderId);
                if (order == null)
                    throw new AppException(string.Format("Order not available for this order: {0}.", payModel.OrderId));

                var existingCharge = GetCharge(payModel.OrderId);
                if (existingCharge != null)
                {
                    if (existingCharge.Status == "succeeded")
                    {
                        throw new AppException(string.Format("Already payment taken for this order: {0}.", payModel.OrderId));
                    }
                }

                var customerId = CreateCustomer(payModel);

                charge = _stripeService.Charge(customerId, payModel);
                SaveCharge(payModel.OrderId, payModel.SenderCognitoId.ToString(), charge);

                // update order status on successful payment
                order.Paid = true;
                _orderService.Update(Convert.ToInt32(order.OrderId), order);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
            return charge;
        }

        private string CreateCustomer(PayModel payModel)
        {
            if (payModel.CardTokenId == null || payModel.CardTokenId == 0)
            {
                if (payModel.IsAllowToSave)
                {
                    var token = _stripeService.CreateStripeToken(new Models.Card()
                    {
                        CardNumder = payModel.CardNumder,
                        CVC = payModel.CVC,
                        EmailId = payModel.EmailId,
                        Month = payModel.Month,
                        Year = payModel.Year,
                    });


                    var customer = _stripeService.CreateStripeCustomer(payModel, token);
                    var CardToken = new CardToken()
                    {
                        CognitoUserId = payModel.SenderCognitoId,
                        CustomerId = customer.Id,
                        Brand = token.Card.Brand,
                        ExpMonth = token.Card.ExpMonth,
                        ExpYear = token.Card.ExpYear,
                        Last4Digit = token.Card.Last4,
                        Type = token.Type
                    };
                    var stripeCustomer = _CardTokenService.Create(CardToken);
                    return customer.Id;
                }
            }
            else if (payModel.CardTokenId != null && payModel.CardTokenId > 0)
            {
                var CardToken = _CardTokenService.GetById(Convert.ToInt64(payModel.CardTokenId));
                return CardToken.CustomerId;
            }

            return null;
        }

        private Models.Charge GetCharge(int OrderId)
        {
            if (_context.Charge.Any(x => x.OrderId == OrderId))
            {
                throw new AppException(string.Format("Charge available for this order: {0}.", OrderId));
            }

            return _context.Charge.FirstOrDefault(x => x.OrderId == OrderId);
        }

        private void SaveCharge(int orderId, string senderCognitoId, Stripe.Charge charge)
        {
            try
            {
                var newCharge = new Models.Charge()
                {
                    OrderId = orderId,
                    Id = charge.Id,
                    Amount = charge.Amount,
                    AmountCaptured = charge.AmountCaptured,
                    AmountRefunded = charge.AmountRefunded,
                    PaymentMethod = charge.PaymentMethod,
                    Paid = charge.Paid,
                    ReceiptEmail = charge.ReceiptEmail,
                    Refunded = charge.Refunded,
                    Status = charge.Status,
                    Captured = charge.Captured,
                    Currency = charge.Currency,
                    FailureMessage = charge.FailureMessage,
                    ReceiptUrl = charge.ReceiptUrl,
                    Created = charge.Created,
                    SenderCognitoId = senderCognitoId,
                    StatementDescriptor = charge.StatementDescriptor,
                    ApplicationFeeAmount = charge.ApplicationFeeAmount,
                    Disputed = charge.Disputed,
                    Description = charge.Description,
                    FailureCode = charge.FailureCode,
                    BalanceTransaction = charge.BalanceTransactionId,
                };
                _context.Add(newCharge);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                Refund(orderId);
                throw new AppException(ex.Message);
            }
        }

        private Models.Refund SaveRefund(int orderId, Stripe.Refund refund)
        {
            var newRefund = new Models.Refund()
            {
                Id = refund.Id,
                OrderId = orderId,
                Amount = refund.Amount,
                Created = refund.Created,
                Currency = refund.Currency,
                PaymentIntent = refund.PaymentIntentId,
                Reason = refund.Reason,
                ReceiptNumber = refund.ReceiptNumber,
                Status = refund.Status,
            };

            try
            {
                _context.Add(newRefund);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
            return newRefund;
        }

        private void UpdateCharge(int orderId, Stripe.Charge charge)
        {
            var existingCharge = _context.Charge.FirstOrDefault(x => x.OrderId == orderId && x.Id == charge.Id);
            if (existingCharge == null)
                throw new AppException(string.Format("Charge not found for this order {0}.", orderId));

            _context.Entry(existingCharge).State = EntityState.Detached;

            existingCharge.Refunded = true;
            existingCharge.AmountRefunded = charge.AmountRefunded;

            _context.Charge.Update(existingCharge);
            _context.SaveChanges();
            //_context.Dispose();
        }
    }
}
