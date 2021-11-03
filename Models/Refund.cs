using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_bff.Models
{
    public class Refund
    {
        public long OrderId { get; set; }
        public string SenderCognitoId { get; set; }
        public string Id { get; set; }
        public string Object { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BalanceTransaction { get; set; }
        public string Charge { get; set; }
        public DateTime? Created { get; set; }
        public string Currency { get; set; }
        public string PaymentIntent { get; set; }
        public string Reason { get; set; }
        public string ReceiptNumber { get; set; }
        public string Status { get; set; }
    }
}
