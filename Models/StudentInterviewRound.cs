namespace Placement.Portal.Skillup.Models
{
    public class StudentInterviewRound
    {
        public int Id { get; set; }
        public int StudentsInterViewScheduleDetails { get; set; }
        public string feedback { get; set; }
        public long StudentId { get; set; }
        public DateTime? CreateAt { get; set; }
        public long? CreatedBy { get; set; }
        public int? RoundNo { get; set; }
        public int? Status { get; set; }
    }
}
