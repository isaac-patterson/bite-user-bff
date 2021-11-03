using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace user_bff.Models
{
    public class PayModel:Card
    {
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public bool IsAllowToSave { get; set; } = false;
        public long? CardTokenId { get; set; } = null;

        public string EmailId { get; set; }
    }
}
