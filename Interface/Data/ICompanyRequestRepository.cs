using Placement.Portal.Skillup.Models;

namespace Placement.Portal.Skillup.Interface.Data
{
    public interface ICompanyRequestRepository
    {
        Task<List<CompanyRequest>> GetCompanyRequestAsync(int id);
        Task AddCompanyRequestAsync(CompanyRequest companyRequest);
        //Task EditCompanyRequestAsync(CompanyRequest companyRequest);
        //Task DeleteCompanyRequestAsync(CompanyRequest companyRequest);
        List<CandidatesGrid> GetInterviewCandidatesList(int companyId, int collegeId);
    }
}
