using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnbargolAngular.Controllers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AnbargolAngular
{
    [Route("api/[controller]")]
    public class Login : Controller
    {
        // GET: api/<controller>
        [HttpGet("/api/Login")]
        public JsonResult Get()
        {
            Task.Delay(2000).Wait();
            return new JsonResult("boom");
        }
    }


}
