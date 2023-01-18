using Microsoft.EntityFrameworkCore;

namespace Placement.Portal.Skillup.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }        
    }
}
