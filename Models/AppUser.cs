using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Placement.Portal.Skillup.Models
{
    [Table("AppUser")]
    public class AppUser
    {
        [Key, Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int Id { get; set; }

        [Column("UserName")]
        public string UserName { get; set; }

        [Column("PasswordHash")]
        public byte[] PasswordHash { get; set; }

        [Column("PasswordSalt")]
        public byte[] PasswordSalt { get; set; }

        [Column("CompanyOrCollege")]
        public int CompanyOrCollege { get; set; }

        [Column("CollegeId")]
        public int? CollegeId { get; set; }

        [Column("CompanyId")]
        public int? CompanyId { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Column("Status")]
        public bool Status { get; set; }

        [Column("Created")]
        public DateTime Created { get; set; } = DateTime.Now;

        [Column("LastActive")]
        public DateTime LastActive { get; set; } = DateTime.Now;
    }
}
