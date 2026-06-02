using LoanApplicationLibrary.DataAccess;
using LoanApplicationLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoanApplicationLibrary.Data
{
    public class ApplicationData : IApplicationData
    {
        private readonly IDataAccess dataAccess;
        public ApplicationData(IDataAccess db)
        {
            this.dataAccess = db;
        }

        public async Task<IEnumerable<ApplicationResp>> GetApplicationsAsync() =>
            await dataAccess.LoadData<ApplicationResp, dynamic>(
                "dbo.Applications_GetAllFullDetails",
                new { });
        public async Task<IEnumerable<ApplicationResp>> GetApplicationAsynce(int id) => await dataAccess.LoadData<ApplicationResp, dynamic>("dbo.Applications_Get", new{ Id = id });
        public async Task CreateApplication(ApplicationResp obj) => await dataAccess.SaveData("sp.application_insert", obj);
        public async Task UpdateApplication(ApplicationResp application) => await dataAccess.SaveData("dbo.application_Update", application);
        public async Task DeleteApplication(int id) => await dataAccess.SaveData("dbo.application_Delete", new ApplicationResp { Id = id });
    }
}
