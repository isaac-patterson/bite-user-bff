using System;
using System.Collections.Generic;

namespace veni_bff.Models
{
    public class MenuItemOption
    {
        public int MenuItemOptionId { get; set; }
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public List<MenuItemOptionValue> MenuItemOptionValue { get; set; }
    }
}
