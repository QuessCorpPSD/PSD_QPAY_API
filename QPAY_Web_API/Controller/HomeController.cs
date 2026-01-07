using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        [HttpGet]        
        public IActionResult Get()
        {
            return Ok("API is working");
        }
    }
}
