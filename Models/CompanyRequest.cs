using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Placement.Portal.Skillup.Models
{
    [Table("CompanyRequest")]
    public class CompanyRequest
    {
        [Key, Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("CompanyId")]
        public int CompanyId { get; set; }

        [Column("CompanyName")]
        public string CompanyName { get; set; }

        [Column("CollegeId")]
        public int CollegeId { get; set; }

        [Column("CollegeName")]
        public string CollegeName { get; set; }

        [Column("RequestDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RequestDate { get; set; }

        [Column("Department")]
        public string Department { get; set; }

        [Column("CoreAreas")]
        public string CoreAreas { get; set; }

        [Column("CGPAPercent")]
        public decimal Percentage { get; set; }

        [Column("Comments")]
        public string Comments { get; set; }

        //[Column("Created")]
        //public DateTime Created { get; set; } = DateTime.Now;

        //[Column("Modified")]
        //public DateTime LastActive { get; set; } = DateTime.Now;
    }
}
