using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_bff.Models
{
    public class Charge
    {
        public string Id { get; set; }
        public long OrderId { get; set; }
        public string SenderCognitoId { get; set; }
        public string Object { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountCaptured { get; set; }
        public decimal? AmountRefunded { get; set; }
        public string Application { get; set; }
        public decimal? ApplicationFee { get; set; }
        public decimal? ApplicationFeeAmount { get; set; }
        public string BalanceTransaction { get; set; }
        public bool? Captured { get; set; }
        public DateTime? Created { get; set; }
        public string Currency { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        public bool? Disputed { get; set; }
        public string FailureCode { get; set; }
        public string FailureMessage { get; set; }
        public string Invoice { get; set; }
        public string OrderInfo { get; set; }
        public bool? Paid { get; set; }
        public string PaymentMethod { get; set; }
        public string ReceiptEmail { get; set; }
        public string ReceiptUrl { get; set; }
        public bool? Refunded { get; set; }
        public string Review { get; set; }
        public string Shipping { get; set; }
        public string StatementDescriptor { get; set; }
        public string Status { get; set; }
    }
}
