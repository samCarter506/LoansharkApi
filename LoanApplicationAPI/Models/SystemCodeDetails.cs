namespace LoanApplicationAPI.Models
{
    public class SystemCodeDetails
    {
        public int Id { get; set; }
        public int SystemCodeId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int OrderNo { get; set; }
        public string? CreatedById { get; set; }
        public string? ModifiedUserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public ApplicationUser? CreatedUser { get; set; }
        public ApplicationUser? ModifiedUser { get; set; }
        public SystemCode? SystemCode { get; set; }
    }
}
