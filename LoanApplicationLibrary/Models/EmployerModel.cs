using System;
using System.Collections.Generic;
using System.Text;

namespace LoanApplicationLibrary.Models
{
    public class EmployerModel
    {
        public int Id { get; set; }
        public string Employer { get; set; }
        public double NetSalary { get; set; }
        public double GrossSalary { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationModel application { get; set; }
    }
}
