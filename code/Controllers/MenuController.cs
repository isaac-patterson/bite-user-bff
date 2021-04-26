using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using veni_bff.Models;
using veni_bff.Services;

namespace veni_bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private IOptions<Parameters> _options;
        public MenuController(IOptions<Parameters> options)
        {
            Console.WriteLine("Menu Controller!");
            _options = options;
        }

        [HttpGet]
        public ActionResult<List<MenuItem>> Get(int restaurantId)
        {
            List<MenuItem> menuItems;
           
            using (var db = new DBContext(_options))
            {
                menuItems = db.MenuItem
                    .Where(x => x.RestaurantId == restaurantId)
                    .ToList();
            }

            return menuItems;
        }
    }
}
