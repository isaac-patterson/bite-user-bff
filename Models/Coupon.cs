using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_bff.Models
{
    public class Coupon
    {
        public long CouponId { get; set; }
        public Guid RestaurantId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? Discount { get; set; }
        public string DiscountCode { get; set; }
        public bool UserDeleted { get; set; }
    }
}
