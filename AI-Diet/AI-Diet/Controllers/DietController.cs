using Microsoft.AspNetCore.Mvc;

namespace AI_Diet.Controllers
{
    public class DietController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
