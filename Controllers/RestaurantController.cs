using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using user_bff.Services;
using user_bff.Models;
using System.Threading.Tasks;

namespace user_bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        public RestaurantController() {}

        [Authorize]
        [HttpGet]
        public ActionResult<List<Restaurant>> Get([FromServices] DBContext db)
        {
            List<Restaurant> restaurants;
            restaurants = db.Restaurant.ToList(); 
            
            return restaurants;
        }
    }
}
