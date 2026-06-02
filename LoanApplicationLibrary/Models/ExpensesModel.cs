using System;
using System.Collections.Generic;
using System.Text;

namespace LoanApplicationLibrary.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ExpensesModel
    {
        public int Id { get; set; }

        public double rentAmount { get; set; }
        public double foodExpenses { get; set; }
        public double transportExpenses { get; set; }
        public double electricityExpenses { get; set; }
        public double waterExpenses { get; set; }
        public double existingLoanRepayments { get; set; }
        public double otherExpenses { get; set; }

        public int dependents { get; set; }

        public double? monthlyExpenses { get; set; }

        public int ApplicationId { get; set; }

        public ApplicationModel? application { get; set; }

        [NotMapped]
        public double TotalExpense =>
            rentAmount +
            foodExpenses +
            transportExpenses +
            electricityExpenses +
            waterExpenses +
            existingLoanRepayments +
            otherExpenses +
            (monthlyExpenses ?? 0);
    }
}
