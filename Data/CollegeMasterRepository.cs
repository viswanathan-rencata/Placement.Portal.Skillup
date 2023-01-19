using Microsoft.Extensions.Caching.Memory;
using Placement.Portal.Skillup.Interface;
using Placement.Portal.Skillup.Models;
using System.Collections.Generic;

namespace Placement.Portal.Skillup.Data
{
    public class CollegeMasterRepository : ICollegeMasterRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        public CollegeMasterRepository(AppDBContext dbContext,
            IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public List<CollegeMaster> GetAll()
        {
            List<CollegeMaster> list = new();

            list = _memoryCache.Get<List<CollegeMaster>>("CollegeMaster");

            if (list is null)
            {
                list = _dbContext.CollegeMaster.ToList();
                
                _memoryCache.Set("CollegeMaster", list);
            }
            return list;
        }
    }
}
