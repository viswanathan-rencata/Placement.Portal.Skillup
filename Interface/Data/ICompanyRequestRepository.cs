using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Interface.Data
{
    public interface ICompanyRequestRepository
    {
        Task<CompanyRequest> GetCompanyRequestAsync(string name);
        Task AddCompanyRequestAsync(CompanyRequest companyRequest);        
        //Task EditCompanyRequestAsync(CompanyRequest companyRequest);
        //Task DeleteCompanyRequestAsync(CompanyRequest companyRequest);
    }
}
