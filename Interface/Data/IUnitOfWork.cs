namespace Placement.Portal.Skillup.Interface.Data
{
    public interface IUnitOfWork
    {
        ICollegeMasterRepository CollegeMasterRepository { get; }
        IAppUserRepository UserRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
