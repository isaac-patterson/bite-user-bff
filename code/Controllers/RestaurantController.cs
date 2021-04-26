using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using veni_bff.Services;
using veni_bff.Models;
using System.Threading.Tasks;

namespace veni_bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private IOptions<Parameters> _options;
        public RestaurantController(IOptions<Parameters> options)
        {
            Console.WriteLine("Restaurant Controller!");
            _options = options;
        }

        [HttpGet]
        public ActionResult<List<Restaurant>> Get()
        
        {
            List<Restaurant> restaurants;
            using (var db = new DBContext(_options))
            {
                restaurants = db.Restaurant.ToList(); 
            }

            return restaurants;
        }
    }
}
