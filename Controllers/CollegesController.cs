using Microsoft.AspNetCore.Mvc;
using Placement.Portal.Skillup.Interface;
using Placement.Portal.Skillup.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using static Placement.Portal.Skillup.Models.Enum;
using Placement.Portal.Skillup.Interface.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Placement.Portal.Skillup.Controllers
{
    public class CollegesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICollegeMasterRepository _studRepo;
        public CollegesController(IUnitOfWork unitOfWork, ICollegeMasterRepository studRepo)
        {
            _unitOfWork = unitOfWork;
            _studRepo = studRepo;
        }

        [Authorize]
        public ViewResult Index()
        {
            var username = HttpContext.User.Identity.Name;
            var customClaim = HttpContext.User.FindFirst("CompanyOrCollege");
            ViewBag.UserName = username;

            CollegeDetails _collegeDetails = new CollegeDetails();
            _collegeDetails.collegeMaster = new CollegeMaster();
            List<Students> dataMapping = _studRepo.GetStudents();
            if (dataMapping.FirstOrDefault()  != null)
            {               
                _collegeDetails.students = dataMapping.ToList();
            }           
            return View(_collegeDetails);     
             
        }
        
        // GET: CollegeDashboard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View(GetCollegeRegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(CollegeRegisterViewModel model)
        {
            if (model.CollegeId == "0")
            {
                ModelState.AddModelError("College", "Please select any College Name");
            }

            if (ModelState.IsValid)
            {
                var userFromDb = await _unitOfWork.UserRepository.GetUserbyId(model.UserName);

                if (userFromDb is not null)
                {
                    ModelState.AddModelError("UserNameMatchError", "UserName is already exists..!");
                    return View(GetCollegeRegisterViewModel());
                }

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
                    PhoneNumber = model.PhoneNumber
                };

                await _unitOfWork.UserRepository.AddUserAsync(user);

                if (await _unitOfWork.Complete()) return RedirectToAction("Login");
                else
                {
                    return View(GetCollegeRegisterViewModel());
                }
            }
            else
            {
                return View(GetCollegeRegisterViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _unitOfWork.UserRepository.GetUserbyId(model.UserName);

                if (user == null)
                {
                    ModelState.AddModelError("PasswordMatchError", "UserName/Password is incorrect");
                    return View();
                }
                else
                {
                    if (!user.Status)
                    {
                        ModelState.AddModelError("UserInactiveError", $"{model.UserName} is inactive.Please contact administrator. ");
                        return View();
                    }
                    else if (user.CompanyOrCollege == (int)CompanyOrCollege.Company)
                    {
                        ModelState.AddModelError("UserInactiveError", $"{model.UserName} is invalid");
                        return View();
                    }
                }

                using var hmac = new HMACSHA512(user.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

                if (!computedHash.SequenceEqual(user.PasswordHash))
                {
                    ModelState.AddModelError("PasswordMatchError", "UserName/Password is incorrect");
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("CompanyOrCollege", user.CompanyOrCollege.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Dashboard");
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

        private CollegeRegisterViewModel GetCollegeRegisterViewModel()
        {
            var collegeRegisteVM = new CollegeRegisterViewModel();
            var clgList = _unitOfWork.CollegeMasterRepository.GetAll();
            collegeRegisteVM.College = GetDropDownItems(clgList);
            return collegeRegisteVM;
        }
    }
}
