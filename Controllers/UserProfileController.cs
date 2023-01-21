using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserProfileController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Edit()
        {
            var username = HttpContext.User.Identity.Name;
            var user = await _unitOfWork.UserRepository.GetUserbyId(username);
            var model = _mapper.Map<UserProfileViewModel>(user);
            return PartialView("Edit", model);
        }
                
        [HttpPost]
        public async Task<IActionResult> Edit(UserProfileViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = _mapper.Map<AppUser>(model);                
                _unitOfWork.UserRepository.UpdateUser(user);

                await _unitOfWork.Complete();
                ModelState.AddModelError("UpdateSuccess", "User Detail Updated Successfully.!");
                model.IsUpdateSuccess = true;
                return PartialView("Edit", model);
            }

            return PartialView("Edit", model);
        }
    }
}
