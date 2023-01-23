using System.ComponentModel.DataAnnotations.Schema;

namespace Placement.Portal.Skillup.Models
{
    [Table("StudentsInterViewScheduleDetails")]
    public class StudentsInterViewScheduleDetails
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("CollegeId")]
        public long CollegeId { get; set; }

        [Column("StudentId")]
        public long StudentId { get; set; }

        [Column("CompanyId")]
        public long CompanyId { get; set; }

        [Column("CompanyRequestId")]
        public long CompanyRequestId { get; set; }

        [Column("CreateAt")]
        public DateTime? CreateAt { get; set; }

        [Column("CreatedBy")]
        public int? CreatedBy { get; set; }
    }
}
