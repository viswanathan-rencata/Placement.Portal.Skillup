using Placement.Portal.Skillup.Controllers;

namespace Placement.Portal.Skillup.Models
{
    public class CollegeDetails
    {
        public CollegeMaster collegeMaster { get; set; }
        public List<Students> students { get; set; }

        public List<CompanyRequest> companyRequest { get; set; }

        public List<StudentsDropdown> studentsDropdown { get; set; }
        public List<StudentInterviewRound> StudentInterviewRound { get; set; }
    }
}
