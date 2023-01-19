using Microsoft.Extensions.Caching.Memory;
using Placement.Portal.Skillup.Interface;
using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;
        private readonly IMemoryCache _memoryCache;
        public UnitOfWork(AppDBContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public IAppUserRepository UserRepository => new AppUserRepository(_context);
        public ICollegeMasterRepository CollegeMasterRepository => new CollegeMasterRepository(_context, _memoryCache);

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
