using LoanApplicationLibrary.Models;

namespace LoanApplicationLibrary.Data
{
    public interface IApplicationData
    {
        Task CreateApplication(ApplicationResp obj);
        Task DeleteApplication(int id);
        Task<IEnumerable<ApplicationResp>> GetApplicationAsynce(int id);
        Task<IEnumerable<ApplicationResp>> GetApplicationsAsync();
        Task UpdateApplication(ApplicationResp application);
    }
}