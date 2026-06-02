using LoanApplicationLibrary.Models;

namespace LoanApplicationLibrary.Data
{
    public interface IEmployerData
    {
        Task CreateEmployer(EmployerModel obj);
        Task DeleteEmployer(int id);
        Task<IEnumerable<EmployerModel>> GetEmployerAsynce(int id);
        Task<IEnumerable<EmployerModel>> GetEmployersAsync();
        Task UpdateEmployer(EmployerModel emplyoer);
    }
}