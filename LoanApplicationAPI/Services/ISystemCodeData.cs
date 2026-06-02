using LoanApplicationAPI.Models;

namespace LoanApplicationAPI.Services
{
    public interface ISystemCodeData
    {
        Task CreateSystemCode(SystemCode obj, string userId);
        Task DeleteSystemCode(string userId, int Id);
        Task<SystemCode> GetSystemCode(int Id);
        Task<List<SystemCode>> GetSystemCodes();
        Task UpdateSystemCode(SystemCode obj, string userId, int Id);
    }
}