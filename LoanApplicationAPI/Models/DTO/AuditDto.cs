namespace LoanApplicationAPI.Models.DTO
{
    public class AuditDto
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public string TransactionData { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CreatedById { get; set; }
        public string? ModifiedById { get; set; }
 
    }
}
