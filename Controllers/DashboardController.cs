using Microsoft.AspNetCore.Mvc;

namespace Placement.Portal.Skillup.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
