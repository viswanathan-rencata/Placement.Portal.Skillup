using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
            _memoryCache.Remove("AppUser");
            _memoryCache.Set("AppUser", dataMap);
            return dataMap;
        }

        public async Task<List<AppUser>> GetAllUser()
        {
            return await _dbContext.AppUser.ToListAsync();
        }

        public void UpdateUser(AppUser user)
        {
            var userFromDB = _dbContext.AppUser.SingleOrDefault(x => x.UserName == user.UserName.ToLower());

            if(userFromDB != null)
            {
                userFromDB.Email = user.Email;
                userFromDB.PhoneNumber = user.PhoneNumber;
                userFromDB.Education = user.Education;
                userFromDB.AddressLine1 = user.AddressLine1;
                userFromDB.AddressLine2 = user.AddressLine2;
                userFromDB.Postcode = user.Postcode;
                userFromDB.Area = user.Area;
                userFromDB.Country = user.Country;
                userFromDB.State = user.State;
                userFromDB.City = user.City;
            }
        }        
    }
}
