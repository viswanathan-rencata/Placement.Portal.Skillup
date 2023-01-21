namespace Placement.Portal.Skillup.Interface.Data
{
    public interface IUnitOfWork
    {
        ICollegeMasterRepository CollegeMasterRepository { get; }
        IAppUserRepository UserRepository { get; }
        ICompanyMasterRepository CompanyMasterRepository { get; }
        ICompanyRequestRepository CompanyRequestRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
