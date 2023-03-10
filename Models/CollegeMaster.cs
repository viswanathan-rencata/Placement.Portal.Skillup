
using System.ComponentModel.DataAnnotations.Schema;

namespace Placement.Portal.Skillup.Models
{
    [Table("CollegeMaster")]
    public class CollegeMaster
    {
        public CollegeMaster()
        {
            Status = true;
        }

        [Column("ID")]
        public int ID { get; set; }

        [Column("Code")] 
        public string Code { get; set; }
        
        [Column("Name")]
        public string Name { get; set; }
        
        [Column("Status")]
        public bool Status { get; set; }
    }
}
