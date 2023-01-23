using Placement.Portal.Skillup.Controllers;
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
        public string ProcessTemplate(string data, int studId, int collegeId);
        public bool AddStudentsInterViewScheduleDetails(List<StudentsInterViewScheduleDetails> student);
        public List<StudentInterviewRound> GetStudentInterviewRound(int colegeId,int studId);

        
    }
}
