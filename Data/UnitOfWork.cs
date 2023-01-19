using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;        
        public UnitOfWork(AppDBContext context)
        {
            _context = context;            
        }

        public IAppUserRepository UserRepository => new AppUserRepository(_context);        

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
