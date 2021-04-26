using System;

namespace veni_bff.Models
{
    public class MenuItem
    {
        public int RestaurantId { get; set; }
        public int MenuItemId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}