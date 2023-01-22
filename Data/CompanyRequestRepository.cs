using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Models;
using System.Collections.Generic;
using System.ComponentModel.Design;

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

        public List<CompanyRequest> GetCompanyRequest(int companyId)
        {
            var companyRequest = _dbContext.CompanyRequest;
            var companyMaster = _dbContext.CompanyMaster;
            var collegeMaster = _dbContext.CollegeMaster;

            var companyRequestValue = (from cr in companyRequest
                               join cm in companyMaster on cr.CompanyId equals cm.ID
                               join colm in collegeMaster on cr.CollegeId equals colm.ID
                               select new CompanyRequest
                               {
                                   CompanyName = cm.Name,
                                   CollegeName = colm.Name,
                                   RequestDate = cr.RequestDate,
                                   Department = cr.Department,
                                   CoreAreas = cr.CoreAreas,
                                   Percentage = cr.Percentage,
                                   Comments = cr.Comments

                               }).ToList();

            return companyRequestValue;
            //return await _dbContext.CompanyRequest.Where(x => x.CompanyId == companyId).ToListAsync();
        }

       
    }
}
