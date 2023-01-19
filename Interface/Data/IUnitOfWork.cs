namespace Placement.Portal.Skillup.Interface.Data
{
    public interface IUnitOfWork
    {
        IAppUserRepository UserRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
