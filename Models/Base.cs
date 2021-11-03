using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace user_bff.Models
{
    public class OrderStatus
    {
        public long OrderId { get; set; }
        public string PickupName { get; set; }
        public string Status { get; set; }
        public DateTime? PickupDate { get; set; }
        public bool Paid { get; set; }
    }
}
