using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Interface
{
    public interface ICollegeMasterRepository
    {
        List<CollegeMaster> GetAll();
        public List<Students> GetStudents(int CollegeId);
        public Students GetStudent(int studId);
        public bool AddStudents(Students Student);
        public CollegeMaster GetCollegeById(int colegeId);
        public List<CompanyRequest> GetCompanyRequestByCollegeId(int colegeId);


    }
}
