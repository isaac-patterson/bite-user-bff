using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_bff.Helpers;
using user_bff.Models;
using user_bff.Services;

namespace user_bff.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        public MenuController() { }

        [HttpGet("{restaurantId}")]
        public ActionResult<List<MenuItem>> GetByRestaurantId([FromServices] DBContext db, Guid restaurantId)
        {
            List<MenuItem> menuItems;

            try 
            {
                menuItems = db.MenuItem
                        .Include(x => x.MenuItemOptions)
                        .ThenInclude(x => x.MenuItemOptionValues)
                        .Where(x => x.RestaurantId == restaurantId)
                        .ToList();  

                return menuItems;
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{menuItemId}")]
        public ActionResult<MenuItem> GetByMenuItemId([FromServices] DBContext db, int menuItemId)
        {
            MenuItem menuItems;
            try {

                menuItems = db.MenuItem
                        .Include(x => x.MenuItemOptions)
                        .ThenInclude(x => x.MenuItemOptionValues)
                        .Where(x => x.menuItemId == menuItemId)
                        .FirstOrDefault();

                //List<MenuItemOption> menuItemOptions = db.MenuItemOption
                //    .Include(x => x.MenuItemOptionValues)
                //    .Where(x => x.MenuItemId == menuItems.menuItemId).ToList();

                //menuItems.MenuItemOptions = menuItemOptions;

                return menuItems;
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
