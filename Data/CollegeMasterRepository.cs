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
        public CollegeMaster GetCollegeById(int colegeId)
        {
            CollegeMaster dataCollege = new();

            List<CollegeMaster> list = new();
            list = _dbContext.CollegeMaster.ToList();
            if (list.Count > 0)
            {
                dataCollege = list.Where(x => x.ID == colegeId).FirstOrDefault();
            }
            return dataCollege;
        }
        public List<CompanyRequest> GetCompanyRequestByCollegeId(int colegeId)
        {
           
            List<CompanyRequest> dataCompanyRequest = new();
            List<CompanyRequest> list = new();
            list = _dbContext.CompanyRequest.ToList();
            if (list.Count > 0)
            {
                dataCompanyRequest = list.Where(x => x.CollegeId == colegeId).ToList();
            }
            return dataCompanyRequest;
        }

        public Students GetStudent(int studId)
        {
            throw new NotImplementedException();
        }
        public List<Students> GetStudents(int CollegeId)
        {
            List<Students> list = new(); 

            if (_dbContext.Students.FirstOrDefault() != null)
            {
                list = _dbContext.Students.Where(x => x.CollegeId == CollegeId).ToList();
            }

            return list;
        }
        public bool AddStudents(Students Student)
        {
            _dbContext.Students.Add(Student);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
