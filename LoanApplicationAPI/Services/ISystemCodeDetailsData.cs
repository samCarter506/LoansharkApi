using LoanApplicationAPI.Models;

namespace LoanApplicationAPI.Services
{
    public interface ISystemCodeDetailsData
    {
        Task CreateSystemCodeDetails(SystemCodeDetails obj, string userId);
        Task DeleteSystemCodeDetails(string userId, int Id);
        Task<SystemCodeDetails> GetSystemCodeDetail(int Id);
        Task<List<SystemCodeDetails>> GetSystemCodeDetails();
        Task UpdateSystemCodeDetails(SystemCodeDetails obj, string userId, int Id);
    }
}