using LoanApplicationLibrary.DataAccess;
using LoanApplicationLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoanApplicationLibrary.Data
{
    public class EmployerData : IEmployerData
    {
        private readonly IDataAccess dataAccess;
        public EmployerData(IDataAccess db)
        {
            dataAccess = db;
        }
        public async Task<IEnumerable<EmployerModel>> GetEmployersAsync() => await dataAccess.LoadData<EmployerModel, dynamic>("dbo.employer_All", new { });
        public async Task<IEnumerable<EmployerModel>> GetEmployerAsynce(int id) => await dataAccess.LoadData<EmployerModel, dynamic>("dbo.employer_Get", new EmployerModel { Id = id });
        public async Task CreateEmployer(EmployerModel obj) => await dataAccess.SaveData("sp.employer_insert", obj);
        public async Task UpdateEmployer(EmployerModel employer) => await dataAccess.SaveData("dbo.employer_Update", employer);
        public async Task DeleteEmployer(int id) => await dataAccess.SaveData("dbo.employer_Delete", new EmployerModel { Id = id });
    }
}
