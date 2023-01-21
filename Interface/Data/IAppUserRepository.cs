using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Interface.Data
{
    public interface IAppUserRepository
    {
        Task AddUserAsync(AppUser user);
        Task<AppUser> GetUserbyId(string userName);
        Task<List<AppUser>> GetAllUser();
        void UpdateUser(AppUser user);
    }
}
