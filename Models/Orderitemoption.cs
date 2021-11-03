using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace user_bff.Models
{
    public class OrderItemOption
    {
        public long OrderItemOptionId { get; set; }
        public long? OrderItemId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public decimal? Price { get; set; }

    }
}
