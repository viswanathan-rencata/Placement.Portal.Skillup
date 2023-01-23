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
using Microsoft.AspNetCore.Html;
using SelectPdf;
using System.Drawing.Printing;
using System.Diagnostics.Metrics;
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
        public JsonResult AjaxStudents(string data)
        {
            CollegeDetails _collegeDetails = new CollegeDetails();
            var colId = HttpContext.User.FindFirst("CollegeId");
            var Id = Convert.ToInt32(colId.Value);
            _collegeDetails.students = _studRepo.GetStudents(Convert.ToInt32(colId.Value)).ToList();
            List<StudentsDropdown> lstStudentsDropdown = new List<StudentsDropdown>();
            foreach (var item in _collegeDetails.students)
            {
                string middelData = "";
                if (!string.IsNullOrEmpty(item.MiddleName))
                {
                    middelData = item.MiddleName;
                }
                StudentsDropdown vStudentsDropdown = new StudentsDropdown();
                vStudentsDropdown.Id = Convert.ToInt32(item.Id);
                vStudentsDropdown.Name = item.FirstName + middelData + item.LastName + "|" + item.Id;
                lstStudentsDropdown.Add(vStudentsDropdown);
            }
            _collegeDetails.studentsDropdown = lstStudentsDropdown;
            return Json(new { data = _collegeDetails });
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
        [HttpPost]
        public JsonResult AjaxStudentIds(string Ids, string companyId, string companyRequestId)
        {
            var colId = HttpContext.User.FindFirst("CollegeId");

            List<string> lstIds = new List<string>();
            List<StudentsInterViewScheduleDetails> studentsInterViewScheduleDetails = new List<StudentsInterViewScheduleDetails>();
            var vIds = Ids.Split(',');

            foreach (var item in vIds)
            {
                if (item.Trim() != "")
                {
                    lstIds.Add(item.Split('|')[1]);
                }

            }
            var datalst = lstIds.Distinct().ToList();

            foreach (var studid in datalst)
            {
                var singleStudentsInterViewScheduleDetails = new StudentsInterViewScheduleDetails()
                {
                    CollegeId = Convert.ToInt64(colId.Value),
                    StudentId = Convert.ToInt64(studid),
                    CompanyId = Convert.ToInt64(companyId),
                    CompanyRequestId = Convert.ToInt64(companyRequestId),
                    CreateAt = DateTime.Now,
                    CreatedBy = Convert.ToInt32(companyId)
                };

                studentsInterViewScheduleDetails.Add(singleStudentsInterViewScheduleDetails);
            }
            bool retVal = _studRepo.AddStudentsInterViewScheduleDetails(studentsInterViewScheduleDetails);
            return Json("Approved");
        }

        [HttpPost]
        public JsonResult GetStudentStatus(string studId)
        {
            List<StudentInterviewRound> lstStudInt = new List<StudentInterviewRound>();

            var colId = HttpContext.User.FindFirst("CollegeId");
            lstStudInt = _studRepo.GetStudentInterviewRound(Convert.ToInt32(colId.Value), Convert.ToInt32(studId));
            return Json(lstStudInt);
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

        [HttpPost]
        public JsonResult GeneratePDF(string data, string studId)
        {
            data = "";
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            //// set converter options
            converter.Options.PdfPageSize = PdfPageSize.Letter;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            data = GetOfferTemplate();
            var colId = HttpContext.User.FindFirst("CollegeId");
            string output = _studRepo.ProcessTemplate(data, Convert.ToInt32(studId), Convert.ToInt32(colId.Value.ToString()));
            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(output);

            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            return Json(new { data = pdf });
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

        private string GetOfferTemplate()
        {
            return
                @"<!DOCTYPE html>
                <html lang=""en"">
                <head>
                  <meta charset=""UTF-8"">
                  <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                  <script src=""https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.0.1/js/bootstrap.bundle.min.js""
                    integrity=""sha512-sH8JPhKJUeA9PWk3eOcOl8U+lfZTgtBXD41q6cO/slwxGHCxKcW45K4oPCUhHG7NMB4mbKEddVmPuTXtpbCbFA==""
                    crossorigin=""anonymous"" referrerpolicy=""no-referrer""></script>
                  <link rel=""stylesheet"" href=""https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.0.1/css/bootstrap.min.css""
                    integrity=""sha512-Ez0cGzNzHR1tYAv56860NLspgUGuQw16GiOOp/I2LuTmpSK9xDXlgJz3XN4cnpXWDmkNBKXR/VDMTCnAaEooxA==""
                    crossorigin=""anonymous"" referrerpolicy=""no-referrer"" />
                  <link rel=""stylesheet"" href=""app.css"">
                  <title>Document</title>
                </head>
                <body>
                  <div class=""conatiner"" style=""margin: 50px 100px;border-style:groove;padding-left: 50px;padding-bottom: 50px;"">
                    <div class=""row"">                      
                      <div class=""col"">
                        <h1>Employment Offer Letter</h1>
                      </div>                      
                    </div>
                    <div class=""row"">
                      <div class=""col""></div>
                      <div class=""col""></div>
                      <div class=""col center"">
                        <h3>[CompanyName]</h3>
                        <br>
                        <p style=""padding-left: 50px;"">[OfferDate]</p>
                      </div>
                    </div>
                    <div class=""row"">
                      <div class=""col"">
                        [CandidateName]<br>
                        [CandidateAddress]<br>
                        [CandidateCityStateZip]<br>
                      </div>
                    </div>
                    <div class=""row"" style=""padding: 150px 0px 50px 0px;"">
                      <div class=""col"">
                        Dear [CandidateName],
                      </div>
                    </div>
                    <div class=""row"">
                      <div class=""col"">
                        <p>
                          We are pleased to offer you the full-time position of software Trainee at [CompanyName] with a start date of
                          [Startdate], contingent upon [background check, I-9 form, etc.].<br>
                          You will be reporting directly to manager at [Workplacelocation]. We believe your skills and experience are an
                          excellent match for our company. this role, you will be <br>
                          required to researching, investigating and fixing a wide range of technical issues.The annual starting salary
                          for this position is 2,15,000 to be paid on a monthly <br>
                          basis by direct deposit to your account. In addition to this starting salary, we’re offering you stock
                          options, bonuses, commission structures, etc (if applicable).<br>
                          Your employment with [CompanyName] will be on an at-will basis, which means you and the company are free to
                          terminate the employment relationship at any time for any reason. <br>
                          This letter is not a contract or guarantee of employment for a definitive period of time.As an employee of
                          [CompanyName], you are also eligible for our benefits program, <br>
                          which includes [medical insurance, 401(k), vacation time, etc.], and other benefits which will be described in
                          more detail in the employee handbook.<br>
                          Please confirm your acceptance of this offer by signing and returning this letter by
                          [OfferExpirationDate].<br>
                          We are excited to have you join our team! If you have any questions, please feel free to reach out at any
                          time.<br>
                        </p>
                      </div>
                    </div>
                    <div class=""row"" style=""padding-top: 50px;"">
                      <div class=""col"">
                        Sincerely,<br>
                        [Signature]
                      </div>
                    </div>
                  </div>
                </body>
                </html>
                ";
        }

        //private string ProcessTemplate(string data, string CompanyName, string CandidateName, string CandidateAddress
        //    ,string CandidateCityStateZip, string Workplacelocation, string Signature) 
        //{
        //    data = data.Replace("[CompanyName]", CompanyName);
        //    data = data.Replace("[OfferDate]", DateTime.Now.ToString("MM/dd/yyyy"));
        //    data = data.Replace("[CandidateName]", CandidateName);
        //    data = data.Replace("[CandidateAddress]", CandidateAddress);
        //    data = data.Replace("[CandidateCityStateZip]", CandidateCityStateZip);
        //    data = data.Replace("[Startdate]", DateTime.Now.AddDays(60).ToString("MM/dd/yyyy"));
        //    data = data.Replace("[Workplacelocation]", Workplacelocation);
        //    data = data.Replace("[OfferExpirationDate]", DateTime.Now.AddDays(30).ToString("MM/dd/yyyy"));
        //    data = data.Replace("[Signature]", Signature);
        //    return data;
        //}
    }


}
