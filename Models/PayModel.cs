using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace user_bff.Models
{
    public class PayModel
    {
     
        public string CardNumder { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string CVC { get; set; }

        public int Amount { get; set; }

        public string OrderId { get; set; }
    }
}
