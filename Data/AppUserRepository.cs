using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Models;
using System.Collections.Generic;

namespace Placement.Portal.Skillup.Data
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        public AppUserRepository(AppDBContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public async Task AddUserAsync(AppUser user)
        {
            await _dbContext.AppUser.AddAsync(user);
        }

        public async Task<AppUser> GetUserbyId(string userName)
        {
            var dataMap = await _dbContext.AppUser.SingleOrDefaultAsync(x => x.UserName == userName.ToLower());
            _memoryCache.Set("AppUser", dataMap);
            return dataMap;
        }
    }
}
