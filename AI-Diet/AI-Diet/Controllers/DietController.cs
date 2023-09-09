using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AI_Diet.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DietController : ControllerBase
    {
        [HttpGet("Get")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
