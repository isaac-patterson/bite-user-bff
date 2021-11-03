using System;
using Microsoft.AspNetCore.Mvc;

namespace user_bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // Used as health check from aws ec2
        [HttpGet]
        public ActionResult<string> Get()
        {
            Console.WriteLine("Values Controller!");
            return  "healthy";
        }
    }
}
