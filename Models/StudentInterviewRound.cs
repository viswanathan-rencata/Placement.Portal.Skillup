using System.ComponentModel.DataAnnotations.Schema;

namespace Placement.Portal.Skillup.Models
{
    [Table("StudentInterviewRound")]
    public class StudentInterviewRound
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("StudentsInterViewScheduleDetails")]
        public int StudentsInterViewScheduleDetails { get; set; }

        [Column("StudentId")]
        public long StudentId { get; set; }

        [Column("Feedback")]
        public string Feedback { get; set; }

        [Column("RoundNo")]
        public int? RoundNo { get; set; }

        [Column("Status")]
        public int? Status { get; set; }

        [Column("CreateAt")]
        public int? CreateAt { get; set; }

        [Column("CreatedBy")]
        public int? CreatedBy { get; set; }
    }
}
