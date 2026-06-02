using LoanApplicationAPI.Models;

namespace LoanApplicationAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
    }
}