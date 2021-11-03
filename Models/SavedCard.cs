using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_bff.Models
{
    public class SavedCard
    {
        public long Id { get; set; }

        public string Brand { get; set; }

        public long ExpMonth { get; set; }

        public long ExpYear { get; set; }

        public string Last4 { get; set; }

        public string Type { get; set; }

    }
}
