using System.ComponentModel.DataAnnotations;

namespace LoanApplicationAPI.Models.DTO
{
    public class SystemCodeDto
    {
        
        [Required]

        [Display(Name = "Code")]
        public string Code { get; set; }
        [Required]

        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]

        [Display(Name = "OrderNo")]
        public int OrderNo { get; set; }

        public string CreatedById { get; set; }
        public string? ModifiedUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public ApplicationUser CreatedUser { get; set; }
        public ApplicationUser? ModifiedUser { get; set; }

    }
}
