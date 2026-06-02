using System;
using System.Collections.Generic;
using System.Text;

namespace LoanApplicationLibrary.Models
{
    public class LoanModel
    {
        public int Id { get; set; }

        public int? loanTermMonths { get; set; }

        public int? interestRate { get; set; }

        public decimal Amount { get; set; }

        public string? Status { get; set; }
        public decimal? monthlyPayment { get; set; }
        public decimal? RemainingBalance { get; set; }

        public int ApplicationId { get; set; }

        public ApplicationModel? application { get; set; }
    }
}
