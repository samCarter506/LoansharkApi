namespace LoanApplicationAPI.Models
{
    public class UserAudit
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public string TransactionData { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CreatedById { get; set; }
        public string? ModifiedById { get; set; }
        public ApplicationUser CreatedUser { get; set; }
        public ApplicationUser? ModifiedUser { get; set; }
    }
}
