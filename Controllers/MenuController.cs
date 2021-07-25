using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_bff.Models;
using user_bff.Services;

namespace user_bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        public MenuController() { }

        [HttpGet]
        [Authorize]
        public ActionResult<List<MenuItem>> Get([FromServices] DBContext db, int restaurantId)
        {
            List<MenuItem> menuItems;
           
            menuItems = db.MenuItem
                    .Include(x => x.MenuItemOption)
                    .ThenInclude(x => x.MenuItemOptionValue)
                    .Where(x => x.RestaurantId == restaurantId)
                    .ToList();

            return menuItems;
        }
    }
}
