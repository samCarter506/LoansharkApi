using Microsoft.AspNetCore.Identity;

namespace LoanApplicationAPI.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? ProvinceId { get; set; }
        public int? GenderId { get; set; }
        public int? RaceId { get; set; }
        public int? HomeLanguage { get; set; }
        public SystemCodeDetails? Province { get; set; }
        public SystemCodeDetails? Gender { get; set; }
        public SystemCodeDetails? Language { get; set; }
        public SystemCodeDetails? Race { get; set; }
    }
}
