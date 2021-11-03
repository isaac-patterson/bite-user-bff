using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace user_bff.Models
{
    public class CardToken
    {
        public long CardTokenId { get; set; }
        public Guid CognitoUserId { get; set; }
        public string CustomerId { get; set; }
        public string Brand { get; set; }
        public string Last4Digit { get; set; }
        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
