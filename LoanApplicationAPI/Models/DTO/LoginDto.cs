using System.Text.Json.Serialization;

namespace LoanApplicationAPI.Models.DTO
{
    public class LoginDto
    {
   
        public string email { get; set; }
         public string password { get; set; }
    }
}
