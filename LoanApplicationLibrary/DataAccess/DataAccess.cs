using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LoanApplicationLibrary.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration _config;
        public DataAccess(IConfiguration data)
        {
            _config = data;
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedures, U parameters, string connectionId = "DefaultConnection")
        {
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId)))
            {
                return await connection.QueryAsync<T>(storedProcedures, parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public async Task SaveData<T>(string storedProcedures, T parameters, string connectionId = "DefaultConnection")
        {
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId)))
            {
                await connection.ExecuteAsync(storedProcedures, parameters, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
