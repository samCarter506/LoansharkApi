using System.Text.Json.Serialization;

namespace LoanApplicationAPI.Models.DTO
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Role { get; set;  } = string.Empty;



    }
}
