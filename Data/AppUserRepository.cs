using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Data
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AppDBContext _dbContext;
        public AppUserRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserAsync(AppUser User)
        {
            await _dbContext.AppUser.AddAsync(User);
        }
    }
}
