using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Models;
using System.Collections.Generic;

namespace Placement.Portal.Skillup.Data
{
    public class CompanyRequestRepository : ICompanyRequestRepository
    {
        private readonly AppDBContext _dbContext;
        public CompanyRequestRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCompanyRequestAsync(CompanyRequest companyRequest)
        {
            await _dbContext.AddAsync(companyRequest);
        }

        public async Task<CompanyRequest> GetCompanyRequestAsync(string name)
        {
            return await _dbContext.CompanyRequest.SingleOrDefaultAsync(x => x.CompanyName == name);
        }
    }
}
