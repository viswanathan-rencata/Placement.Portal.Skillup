using Microsoft.Extensions.Caching.Memory;
using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Data
{
    public class CompanyMasterRepository : ICompanyMasterRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        public CompanyMasterRepository(AppDBContext dbContext,
            IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public List<CompanyMaster> GetAll()
        {
            List<CompanyMaster> list = new();

            list = _memoryCache.Get<List<CompanyMaster>>("CompanyMaster");

            if (list is null)
            {
                list = _dbContext.CompanyMaster.ToList();

                _memoryCache.Set("CompanyMaster", list);
            }
            return list;
        }
    }
}
