using LoanApplicationLibrary.Models;

namespace LoanApplicationLibrary.Data
{
    public interface IBankingData
    {
        Task CreateBanking(BankingModel obj);
        Task DeleteBanking(int id);
        Task<IEnumerable<BankingModel>> GetBankingAsynce(int id);
        Task<IEnumerable<BankingModel>> GetBankingsAsync();
        Task UpdateBanking(BankingModel banking);
    }
}