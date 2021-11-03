using System;
using System.Collections.Generic;

namespace user_bff.Models
{
    public class MenuItemOption
    {
        public int MenuItemOptionId { get; set; }
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public virtual List<MenuItemOptionValue> MenuItemOptionValues { get; set; }
    }
}
