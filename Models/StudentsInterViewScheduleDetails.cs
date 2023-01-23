namespace Placement.Portal.Skillup.Models
{
    public class StudentsInterViewScheduleDetails
    {
        public int Id { get; set; }
        public long CollegeId { get; set; }
        public long StudentId { get; set; }
        public long CompanyId { get; set; }
        public long CompanyRequestId { get; set; }
        public DateTime CreateAt { get; set; }
        public int CreatedBy { get; set; }
    }
}
