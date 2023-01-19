using Microsoft.AspNetCore.Mvc;
using Placement.Portal.Skillup.Interface;
using Placement.Portal.Skillup.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Placement.Portal.Skillup.Controllers
{
    public class CollegesController : Controller
    {
        private readonly ICollegeMasterRepository _collegeMasterRepository;
        public CollegesController(ICollegeMasterRepository collegeMasterRepository)
        {
            _collegeMasterRepository = collegeMasterRepository;
        }
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
            var clgList = _collegeMasterRepository.GetAll();
            collegeRegisteVM.College = GetDropDownItems(clgList);
            return View(collegeRegisteVM);
        }

        [HttpPost]
        public IActionResult Register(CollegeRegisterViewModel model)
        {
            if(model.CollegeId == "0")
            {
                ModelState.AddModelError("CollegeValidation", "Please select any College Name");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var collegeRegisteVM = new CollegeRegisterViewModel();
                var clgList = _collegeMasterRepository.GetAll();
                collegeRegisteVM.College = GetDropDownItems(clgList);
                return View(collegeRegisteVM);
            }
        }

        private List<SelectListItem> GetDropDownItems(List<CollegeMaster> list)
        {
            List<SelectListItem> dropdownList = new();
            dropdownList.Add(new SelectListItem { Text = "Select", Value = "0", Selected = true });
            foreach (var item in list)
            {
                dropdownList.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
            }
            return dropdownList;
        }
    }
}
