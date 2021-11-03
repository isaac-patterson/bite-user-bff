using System;
using System.Collections.Generic;

#nullable disable

namespace user_bff.Models
{
    public partial class MenuItem
    {
        public long menuItemId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid RestaurantId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public short? AvailableOptionsCount { get; set; }
        public virtual List<MenuItemOption> MenuItemOptions { get; set; }
    }
}