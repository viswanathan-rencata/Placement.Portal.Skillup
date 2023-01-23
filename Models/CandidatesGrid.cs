namespace Placement.Portal.Skillup.Models
{
    public class CandidatesGrid
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public char Gender { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Dept { get; set; }
        public long PhoneNumber { get; set; }
        public decimal Percentage { get; set; }
        public int? StudentsInterViewScheduleDetailsId { get; set; }
    }
}
