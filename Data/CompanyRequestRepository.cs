using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Models;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

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

        public async Task<List<CompanyRequest>> GetCompanyRequestAsync(int companyId)
        {
            return await _dbContext.CompanyRequest.Where(x => x.CompanyId == companyId).ToListAsync();
        }

        public List<CandidatesGrid> GetInterviewCandidatesList(int companyId, int collegeId)
        {
            List<CandidatesGrid> List = new();
            var IniterviewSchedule = _dbContext.StudentsInterViewScheduleDetails
                .Where(x=>x.CompanyId == companyId && x.CollegeId == collegeId).ToList();

            var StudentIDList = IniterviewSchedule.Select(x => x.StudentId).ToList();

            var StudentList = _dbContext.Students.ToList()
                .Where(x=> StudentIDList.Contains(x.Id)).ToList();

            foreach(var item in StudentList)
            {
                CandidatesGrid obj = new();
                obj.Id = item.Id;
                obj.FirstName = item.FirstName;
                obj.LastName = item.LastName;
                obj.DOB = item.DOB;
                obj.Email = item.Email;
                obj.PhoneNumber = item.PhoneNumber;
                obj.Dept = item.Dept;
                obj.Percentage = item.Percentage;
                obj.StudentsInterViewScheduleDetailsId = IniterviewSchedule.Select(x => x.ID).FirstOrDefault();
                List.Add(obj);
            }

            return List;
        }

       
    }
}
