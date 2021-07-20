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
        public string CognitoUserId { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? PickupDate { get; set; }
        public long? RestaurantId { get; set; }
        public decimal? Total { get; set; }
        public string Currency { get; set; }
        public string Notes { get; set; }
        public List<Orderitem> Orderitems { get; set; }
    }
}
