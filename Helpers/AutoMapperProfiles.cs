using AutoMapper;
using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserProfileViewModel>();
            CreateMap<UserProfileViewModel, AppUser>();
        }
    }
}
