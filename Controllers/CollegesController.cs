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
//using Newtonsoft.Json.Serialization;

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
            var colId = HttpContext.User.FindFirst("CollegeId");
            ViewBag.UserName = username;

            CollegeDetails _collegeDetails = new CollegeDetails();
            _collegeDetails.collegeMaster = _studRepo.GetCollegeById(Convert.ToInt32(colId.Value));
            _collegeDetails.students = _studRepo.GetStudents(Convert.ToInt32(colId.Value)).ToList();
            _collegeDetails.companyRequest = _studRepo.GetCompanyRequestByCollegeId(Convert.ToInt32(colId.Value));
            ViewBag.CollegeName = _collegeDetails.collegeMaster.Name; // _studRepo.GetCollegeById(colId).Name;

            return View(_collegeDetails);

        }
        [HttpPost]
        public JsonResult AjaxStudentPostCall(string FirstName, string MiddleName, string LastName,
            string Gender, string Email, string DOB, string DOJ, string Dept, string ClassName,
            string Address, string PhoneNumber, string Percentage)
        {
            var colId = HttpContext.User.FindFirst("CollegeId");

            var stud = new Students()
            {
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                Address = Address,
                Email = Email,
                ClassName = ClassName,
                Dept = Dept,
                DOB = Convert.ToDateTime(DOB),
                DOJ = Convert.ToDateTime(DOJ),
                Gender = Convert.ToChar(Gender),
                Percentage = Convert.ToDecimal(Percentage),
                PhoneNumber = Convert.ToInt64(PhoneNumber),
                Age = 0,
                CreatedBy = 0,
                IsActive = true,
                Status = "",
                CollegeId = Convert.ToInt32(colId.Value),
                CreatedAt = DateTime.Now
            };

            bool retVal = _studRepo.AddStudents(stud);

            return Json(retVal);
        }
        // GET: CollegeDashboard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Login()
        {
            LoginViewModel model = new();
            return View(model);
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
                    return View(GetCollegeRegisterViewModel(true));
                }

                var allUsers = await _unitOfWork.UserRepository.GetAllUser();

                if (allUsers.Any(x => x.CollegeId == Convert.ToInt16(model.CollegeId)))
                {
                    ModelState.AddModelError("CollegeSelectionError", "Selected college is already registered. Please contact administrator.");
                    return View(GetCollegeRegisterViewModel(true));
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
                    return View(GetCollegeRegisterViewModel(true));
                }
            }
            else
            {
                return View(GetCollegeRegisterViewModel(true));
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
                    model.IsLoginSucceed = false;
                    return View(model);
                }
                else
                {
                    if (!user.Status)
                    {
                        ModelState.AddModelError("UserInactiveError", $"{model.UserName} is inactive.Please contact administrator. ");
                        model.IsLoginSucceed = false;
                        return View(model);
                    }
                    else if (user.CompanyOrCollege == (int)CompanyOrCollege.Company)
                    {
                        ModelState.AddModelError("UserInactiveError", $"{model.UserName} is invalid");
                        model.IsLoginSucceed = false;
                        return View(model);
                    }
                }

                using var hmac = new HMACSHA512(user.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

                if (!computedHash.SequenceEqual(user.PasswordHash))
                {
                    ModelState.AddModelError("PasswordMatchError", "UserName/Password is incorrect");
                    model.IsLoginSucceed = false;
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("CompanyOrCollege", user.CompanyOrCollege.ToString()),
                    new Claim("CollegeId", user.CollegeId.ToString())
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

                model.IsLoginSucceed = false;
                return View(model);
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

        private CollegeRegisterViewModel GetCollegeRegisterViewModel(bool isRegistrationFailed = false)
        {
            var collegeRegisteVM = new CollegeRegisterViewModel();
            var clgList = _unitOfWork.CollegeMasterRepository.GetAll();
            collegeRegisteVM.College = GetDropDownItems(clgList);
            collegeRegisteVM.IsRegistrationFailed = isRegistrationFailed;
            return collegeRegisteVM;
        }
    }
}
