namespace LoanApplicationAPI.Services
{
    public interface ILoanCalculationData
    {
        Task<LoanCalculationModel> LoanCalculations(int id);
    }
}