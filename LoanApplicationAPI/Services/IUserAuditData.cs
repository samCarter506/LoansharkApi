using LoanApplicationAPI.Models;

namespace LoanApplicationAPI.Services
{
    public interface IUserAuditData
    {
        Task CreateAudit(UserAudit audit);
        Task DeleteAudit(int id);
        Task<List<UserAudit>> GetAllAudits();
        Task<UserAudit?> GetAuditById(int id);
        Task<List<UserAudit>> GetUserAudits(string userId);
    }
}