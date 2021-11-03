using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace user_bff.Models
{
    public class Order
    {
        public long OrderId { get; set; }
        public Guid CognitoUserId { get; set; }
        public string PickupName { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? PickedupDate { get; set; }
        public Guid RestaurantId { get; set; }
        public decimal? Total { get; set; }
        public string Currency { get; set; }
        public string Notes { get; set; }
        public long? CouponId { get; set; }
        public bool Paid { get; set; }
        public bool isGift { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Restaurant RestaurantInfo { get; set; }

    }
}
