using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace user_bff.Models
{
    public class OrderItem
    {
        public long OrderItemId { get; set; }
        public long? OrderId { get; set; }
        public decimal? ExtraPrice { get; set; }
        public string Name { get; set; }
        public List<OrderItemOption> OrderItemOptions { get; set; }
    }
}
