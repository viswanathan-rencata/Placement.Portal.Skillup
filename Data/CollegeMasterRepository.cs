using Microsoft.Extensions.Caching.Memory;
using Placement.Portal.Skillup.Controllers;
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

            CompanyMaster companyMaster = new();

            List<CompanyMaster> list = new();
           

            List<CompanyRequest> dataCompanyRequest = new();
            List<CompanyRequest> lstCompanyRequest = new List<CompanyRequest>();
            List<CompanyRequest> listComp = new();
            listComp = _dbContext.CompanyRequest.ToList();
            if (listComp.Count > 0)
            {
                dataCompanyRequest = listComp.Where(x => x.CollegeId == colegeId).ToList();
            }
            foreach (var item in dataCompanyRequest)
            {
                list = _dbContext.CompanyMaster.ToList();
                if (list.Count > 0)
                {
                    companyMaster = list.Where(x => x.ID == item.CollegeId).FirstOrDefault();
                }
                item.CompanyName = companyMaster.Name;

                lstCompanyRequest.Add(item);

            }
            return lstCompanyRequest;
        }

        public Students GetStudent(int studId)
        {
            Students list = new();

            if (_dbContext.Students.Count() > 0)
            {
                list = _dbContext.Students.Where(x => x.Id == studId).FirstOrDefault();
            }

            return list;
        }
        public List<Students> GetStudents(int CollegeId)
        {
            List<Students> list = new();

            if (_dbContext.Students.FirstOrDefault() != null)
            {
                list = _dbContext.Students.Where(x => x.CollegeId == CollegeId).OrderByDescending(y => y.Id).Select(e => e).ToList();

            }

            return list;
        }
        public bool AddStudents(Students Student)
        {
            _dbContext.Students.Add(Student);
            _dbContext.SaveChanges();
            return true;
        }

        public bool AddStudentsInterViewScheduleDetails(List<StudentsInterViewScheduleDetails> student)
        {

            foreach (var item in student)
            {
                _dbContext.StudentsInterViewScheduleDetails.Add(item);
                _dbContext.SaveChanges();
            }

            foreach (var item in student)
            {
                var userFromDB = _dbContext.Students.SingleOrDefault(x => x.Id == item.StudentId);
                userFromDB.Status = "Interview Initiated";
                _dbContext.SaveChanges();
            }
            return true;
        }

        public string ProcessTemplate(string data, int studId, int collegeId)
        {
            var companyData = GetCompanyRequestByCollegeId(collegeId);

            var studData = GetStudent(studId);

            if (companyData.Count > 0)
            {
                data = data.Replace("[CompanyName]", companyData.FirstOrDefault().CompanyName);
            }
            else
            {
                data = data.Replace("[CompanyName]", "");
            }

            data = data.Replace("[OfferDate]", DateTime.Now.ToString("MM/dd/yyyy"));
            data = data.Replace("[CandidateName]", studData.FirstName + " " + studData.MiddleName + " " + studData.LastName);
            data = data.Replace("[CandidateAddress]", studData.Address);
            data = data.Replace("[CandidateCityStateZip]", "Chennai");
            data = data.Replace("[Startdate]", DateTime.Now.AddDays(60).ToString("MM/dd/yyyy"));
            data = data.Replace("[Workplacelocation]", "Chennai");
            data = data.Replace("[OfferExpirationDate]", DateTime.Now.AddDays(30).ToString("MM/dd/yyyy"));
            data = data.Replace("[Signature]", studData.FirstName);
            return data;
        }

        public List<StudentInterviewRound> GetStudentInterviewRound(int colegeId, int studId)
        {
            List<StudentInterviewRound> studentInter = new List<StudentInterviewRound>();
            var data = _dbContext.StudentInterviewRound.Where(x => x.StudentId == studId).ToList();
            if (data.Count > 0)
            {
                studentInter = data.ToList();

            }

            return studentInter;            
        }
    }
}
