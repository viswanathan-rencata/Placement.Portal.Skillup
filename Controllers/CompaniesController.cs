using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Models;
using static Placement.Portal.Skillup.Models.Enum;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Placement.Portal.Skillup.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.User.Identity.Name;
            var customClaim = HttpContext.User.FindFirst("CompanyOrCollege");            

            ViewBag.UserName = username;
            List<CompanyRequest> cr = await _unitOfWork.CompanyRequestRepository.GetCompanyRequestAsync(Convert.ToInt16(HttpContext.User.FindFirst("CompanyId").Value.ToString())); 
            return View(cr);
        }
        
        public IActionResult Login()
        {
            LoginViewModel model = new();

            return View(model);
        }

        public IActionResult Register()
        {
            return View(GetCompanyRegisterViewModel());
        }

        public IActionResult ViewRequests()
        {
            return View("Index");

            //AppDBContext dBContext = new AppDBContext();

            //var data = dBContext.CompanyRequest;
            
            //return View (data.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Register(CompanyRegisterViewModel model)
        {
            if (model.CompanyId == "0")
            {
                ModelState.AddModelError("Company", "Please select any Company Name");
            }

            if (ModelState.IsValid)
            {
                var userFromDb = await _unitOfWork.UserRepository.GetUserbyId(model.UserName);

                if (userFromDb is not null)
                {
                    ModelState.AddModelError("UserNameMatchError", "UserName is already exists..!");
                    return View(GetCompanyRegisterViewModel(true));
                }

                var allUsers = await _unitOfWork.UserRepository.GetAllUser();

                if (allUsers.Any(x => x.CompanyId == Convert.ToInt16(model.CompanyId)))
                {
                    ModelState.AddModelError("CompanySelectionError", "Selected company is already registered. Please contact administrator.");
                    return View(GetCompanyRegisterViewModel(true));
                }

                using var hmac = new HMACSHA512();

                var user = new AppUser
                {
                    UserName = model.UserName.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password)),
                    PasswordSalt = hmac.Key,
                    CompanyOrCollege = (int)CompanyOrCollege.Company,
                    CompanyId = Convert.ToInt16(model.CompanyId),
                    Status = true,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                await _unitOfWork.UserRepository.AddUserAsync(user);

                if (!await _unitOfWork.Complete()) return RedirectToAction("Login");
                else
                {
                    return View(GetCompanyRegisterViewModel(true));
                }                
            }
            else
            {
                return View(GetCompanyRegisterViewModel(true));
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
                    else if (user.CompanyOrCollege == (int)CompanyOrCollege.College)
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
                    new Claim("CompanyId", user.CompanyId.ToString())
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
        private List<SelectListItem> GetDropDownItems(List<CompanyMaster> list)
        {
            List<SelectListItem> dropdownList = new();
            dropdownList.Add(new SelectListItem { Text = "Select", Value = "0", Selected = true });
            foreach (var item in list)
            {
                dropdownList.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
            }
            return dropdownList;
        }

        private CompanyRegisterViewModel GetCompanyRegisterViewModel(bool isRegistrationFailed = false)
        {
            var companyRegisteVM = new CompanyRegisterViewModel();
            var cmpList = _unitOfWork.CompanyMasterRepository.GetAll();
            companyRegisteVM.Company = GetDropDownItems(cmpList);
            companyRegisteVM.IsRegistrationFailed = isRegistrationFailed;
            return companyRegisteVM;
        }

        [HttpPost]
        public async Task<IActionResult> CompanyRequest(CompanyRequestViewModel model)
        {
            if (model.CollegeName == "0")
            {
                ModelState.AddModelError("Company", "Please select any College Name");
            }

            if (ModelState.IsValid)
            {
                
                var companyReq = new CompanyRequest
                {
                    CollegeName = "Southwest Wisconsin Technical College",
                    RequestDate = model.RequestDate,
                    Department = "Electronics",
                    CoreAreas = model.CoreAreas,
                    Percentage = model.CGPAPercent,
                    Comments = model.Comments
                };

                await _unitOfWork.CompanyRequestRepository.AddCompanyRequestAsync(companyReq);

                if (await _unitOfWork.Complete()) return View(GetCompanyRequestViewModel());
                else
                {
                    return View(GetCompanyRequestViewModel());
                }
            }
            else
            {
                return View(GetCompanyRequestViewModel());
            }
        }

        private List<SelectListItem> GetCollegeDropDownItems(List<CollegeMaster> list)
        {
            List<SelectListItem> dropdownList = new();
            dropdownList.Add(new SelectListItem { Text = "Select", Value = "0", Selected = true });
            foreach (var item in list)
            {
                dropdownList.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
            }
            return dropdownList;
        }

        private CompanyRequestViewModel GetCompanyRequestViewModel()
        {
            var companyRequestVM = new CompanyRequestViewModel();
            var cmpReqList = _unitOfWork.CollegeMasterRepository.GetAll();
            companyRequestVM.College = GetCollegeDropDownItems(cmpReqList);
            return companyRequestVM;
        }
    }
}
