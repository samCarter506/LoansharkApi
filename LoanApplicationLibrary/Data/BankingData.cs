using LoanApplicationLibrary.DataAccess;
using LoanApplicationLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoanApplicationLibrary.Data
{
    public class BankingData : IBankingData
    {
        private readonly IDataAccess dataAccess;
        public BankingData(IDataAccess db)
        {
            dataAccess = db;
        }
        public async Task<IEnumerable<BankingModel>> GetBankingsAsync() => await dataAccess.LoadData<BankingModel, dynamic>("dbo.banking_All", new { });
        public async Task<IEnumerable<BankingModel>> GetBankingAsynce(int id) => await dataAccess.LoadData<BankingModel, dynamic>("dbo.banking_Get", new BankingModel { Id = id });
        public async Task CreateBanking(BankingModel obj) => await dataAccess.SaveData("sp.banking_insert", obj);
        public async Task UpdateBanking(BankingModel banking) => await dataAccess.SaveData("dbo.banking_Update", banking);
        public async Task DeleteBanking(int id) => await dataAccess.SaveData("dbo.banking_Delete", new BankingModel { Id = id });

    }
}
