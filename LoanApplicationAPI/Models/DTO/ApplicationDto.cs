using LoanApplicationLibrary.Models;

namespace LoanApplicationAPI.Models.DTO
{

    public class ApplicantDto
    {
        public int Id { get; set; }
        public string NationalId { get;set;  }
        public string Email { get; set; } 
        public string Address { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public  string Cellphone { get; set; }
        public string? Status { get; set; }
        public decimal Amount { get; set; }

    
        public string? AccountType { get; set; }
        public string? BankName { get; set; }
        public string? Branch { get; set; }
        public DateTime? PaymentDate { get; set; }

  
        public string Employer {  get; set; }
        public double NetSalary { get;set;  }
        public double GrossSalary { get; set; }
        
        public int? loanTermMonths { get; set; }
        public int? interestRate { get; set; }
        public double rentAmount { get; set; }
        public double foodExpenses { get; set; }
        public double transportExpenses { get; set; }
        public double electricityExpenses { get; set; }
        public double waterExpenses { get; set; }   
        public double existingLoanRepayments { get; set; }
        public double otherExpenses { get; set; }
        public int dependents {  get; set; }
        public double? monthlyExpenses { get; set; }
        public double? TotalExpense { get; set; }

       public double MonthlyPayment { get; set; }



        public IFormFile IdDocument { get; set; }

        public IFormFile BankStatement { get; set; }

        public IFormFile Payslip { get; set; }

        public List<DocumentsDto> Documents { get; set; } = new();


    }
}
