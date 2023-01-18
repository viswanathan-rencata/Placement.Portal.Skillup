using Microsoft.AspNetCore.Mvc;

namespace Placement.Portal.Skillup.Controllers
{
    public class CompaniesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
