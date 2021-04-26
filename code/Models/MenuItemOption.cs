using System;

namespace veni_bff.Models
{
    public class MenuItemOption
    {
        public int MenuItemOptionId { get; set; }
        public int RestaurantId { get; set; }
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public string Options { get; set; }
    }
}
