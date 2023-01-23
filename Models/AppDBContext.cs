using Microsoft.EntityFrameworkCore;

namespace Placement.Portal.Skillup.Models
{
    public class AppDBContext : DbContext
    {
		public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
		
		public DbSet<CollegeMaster> CollegeMaster { get;set;}
        public DbSet<CompanyMaster> CompanyMaster { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<CompanyRequest> CompanyRequest { get; set; }
        public DbSet<StudentsInterViewScheduleDetails> StudentsInterViewScheduleDetails { get; set; }
        public DbSet<StudentInterviewRound> StudentInterviewRound { get; set; }
    }
}
