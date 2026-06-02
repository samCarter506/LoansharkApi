using Microsoft.EntityFrameworkCore;

namespace LoanApplicationAPI.Services
{
    public class LoanCalculationData : ILoanCalculationData
    {
        private readonly ApplicationDbContext context;

        public LoanCalculationData(ApplicationDbContext db)
        {
            context = db;
        }

        public async Task<LoanCalculationModel> LoanCalculations(int id)
        {
            try
            {
                var application = await context.Applications
                    .Include(a => a.Expenses)
                    .Include(a => a.Loan)
                    .Include(a => a.Employer)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (application == null)
                {
                    throw new Exception("Application not found");
                }

                // =========================
                // AVAILABLE MONEY
                // =========================

                double totalMoneyAvail =
                    Convert.ToDouble(application.Employer.NetSalary)
                    -
                    Convert.ToDouble(application.Expenses.TotalExpense);

                // =========================
                // INTEREST RATE
                // =========================

                double annualInterestRate =
                    Convert.ToDouble(application.Loan.interestRate);

                double rate =
                    annualInterestRate / 100 / 12;

                // =========================
                // LOAN VALUES
                // =========================

                double loanAmount =
                    Convert.ToDouble(application.Loan.Amount);

                int period =
                    Convert.ToInt32(application.Loan.loanTermMonths);

                // =========================
                // MONTHLY PAYMENT
                // =========================

                double monthlyPayment =
                    loanAmount *
                    (
                        rate * Math.Pow(1 + rate, period)
                    )
                    /
                    (
                        Math.Pow(1 + rate, period) - 1
                    );

                // =========================
                // REMAINDER
                // =========================

                double remainder =
                    totalMoneyAvail - monthlyPayment;

                // =========================
                // RETURN MODEL
                // =========================

                LoanCalculationModel loanCalculation =
                    new LoanCalculationModel
                    {
                        MonthlyPayment =
                            Convert.ToDecimal(monthlyPayment),

                        TotalLoanAmount =
                            Convert.ToDecimal(loanAmount),

                        Balance =
                            Convert.ToDecimal(remainder+ loanAmount)
                    };

                return loanCalculation;
            }
            catch (Exception err)
            {
                throw;
            }
        }
    }

    public class LoanCalculationModel
    {
        public decimal MonthlyPayment { get; set; }

        public decimal TotalLoanAmount { get; set; }

        public decimal Balance { get; set; }
    }
}