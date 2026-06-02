using System;
using System.Collections.Generic;
using System.Text;

namespace LoanApplicationLibrary.Models
{
    public class BankingModel
    {
        public int Id { get; set; }
        public string? AccountType { get; set; }
        public string? BankName { get; set; }
        public string? Branch { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationModel application { get; set; }

    }
}
