using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_bff.Models
{
    public class Card
    {
        public Guid SenderCognitoId { get; set; }

        public string CardNumder { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string CVC { get; set; }
        
        public string EmailId { get; set; }
    }
}
