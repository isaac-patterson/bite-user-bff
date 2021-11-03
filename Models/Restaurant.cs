using System;
using System.Collections.Generic;

#nullable disable

namespace user_bff.Models
{
    public partial class Restaurant
    {
        public Guid RestaurantId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string LogoIcon { get; set; }
        public decimal? Offer { get; set; }
        public List<RestaurantOpenDays> RestaurantOpenDays { get; set; }
    }
}
