namespace LoanApplicationLibrary.DataAccess
{
    public interface IDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedures, U parameters, string connectionId = "DefaultConnection");
        Task SaveData<T>(string storedProcedures, T parameters, string connectionId = "DefaultConnection");
    }
}