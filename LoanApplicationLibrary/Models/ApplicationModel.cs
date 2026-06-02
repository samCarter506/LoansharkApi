using LoanApplicationLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoanApplicationLibrary.Models
{
    public class ApplicationModel
    {
        public int Id { get; set; }

        public string NationalId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Cellphone { get; set; } = string.Empty;


        public ICollection<ApplicationDocument> Documents { get; set; }
            = new List<ApplicationDocument>();

        public BankingModel? Banking { get; set; }
        public LoanModel? Loan { get; set; }

        public ExpensesModel? Expenses { get; set; }
        public EmployerModel? Employer { get; set; }
    }
    public class ApplicationResp
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string NationalId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string Cellphone { get; set; }

   

        // Nullable dates
        public DateTime? Date { get; set; }
        public DateTime? PaymentDate { get; set; }

        // Banking
        public string AccountType { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }

        // Employer
        public string Employer { get; set; }
        public double NetSalary { get; set; }
        public double GrossSalary { get; set; }
        //Expense
        public double rentAmount { get; set; }
        public double foodExpenses { get; set; }
        public double transportExpenses { get; set; }
        public double electricityExpenses { get; set; }
        public double waterExpenses { get; set; }
        public double existingLoanRepayments { get; set; }
        public double otherExpenses { get; set; }
        public int dependents { get; set; }
        public double monthlyExpenses { get; set; }
        //loan
        public int loanTermMonths { get; set; }

        public int interestRate { get; set; }

        public decimal Amount { get; set; }

        public string? Status { get; set; }

        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime? UploadedDate { get; set; }
    }
}
