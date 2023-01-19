using Microsoft.AspNetCore.Mvc;
using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Controllers
{
    public class CollegesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            var collegeRegisteVM = new CollegeRegisterViewModel();
            return View(collegeRegisteVM);
        }

        [HttpPost]
        public IActionResult Register(CollegeRegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }            
        }
    }
}
