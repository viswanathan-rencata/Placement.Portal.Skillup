using Microsoft.AspNetCore.Mvc;
using Placement.Portal.Skillup.Interface;
using Placement.Portal.Skillup.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using static Placement.Portal.Skillup.Models.Enum;
using Placement.Portal.Skillup.Interface.Data;

namespace Placement.Portal.Skillup.Controllers
{
    public class CollegesController : Controller
    {
        private readonly ICollegeMasterRepository _collegeMasterRepository;        
        private readonly IUnitOfWork _unitOfWork;
        public CollegesController(ICollegeMasterRepository collegeMasterRepository, IUnitOfWork unitOfWork)
        {
            _collegeMasterRepository = collegeMasterRepository;            
            _unitOfWork = unitOfWork;
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
        public async Task<IActionResult> Register(CollegeRegisterViewModel model)
        {
            if(model.CollegeId == "0")
            {
                ModelState.AddModelError("CollegeValidation", "Please select any College Name");
            }

            if (ModelState.IsValid)
            {
                using var hmac = new HMACSHA512();

                var user = new AppUser
                {
                    UserName = model.UserName.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password)),
                    PasswordSalt = hmac.Key,
                    CompanyOrCollege = (int)CompanyOrCollege.College,
                    CollegeId = Convert.ToInt16(model.CollegeId),
                    Status = true,
                    Email = model.Email,
                    PhoneNumber= model.PhoneNumber
                };

                _unitOfWork.UserRepository.AddUserAsync(user);

              if (await _unitOfWork.Complete()) return RedirectToAction("Login");

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
