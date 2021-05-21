using System;

namespace veni_bff.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public string Address { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}