using System;

namespace user_bff.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Category { get; set; }
    }
}