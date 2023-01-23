using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Interface.Data
{
    public interface ICompanyRequestRepository
    {
        List<CompanyRequest> GetCompanyRequest(int id);
        Task AddCompanyRequestAsync(CompanyRequest companyRequest);        
        //Task EditCompanyRequestAsync(CompanyRequest companyRequest);
        //Task DeleteCompanyRequestAsync(CompanyRequest companyRequest);
    }
}
