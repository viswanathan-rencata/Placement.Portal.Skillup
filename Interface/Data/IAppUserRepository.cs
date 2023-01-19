using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Interface.Data
{
    public interface IAppUserRepository
    {
        Task AddUserAsync(AppUser User);
    }
}
