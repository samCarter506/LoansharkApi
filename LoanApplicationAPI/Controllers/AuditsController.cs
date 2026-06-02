using LoanApplicationAPI.Models.DTO;
using LoanApplicationAPI.Services;
using LoanApplicationLibrary.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditsController : ControllerBase
    {
        public IUserAuditData data { get; set; }
        public AuditsController(ApplicationDbContext dbContext)
        {
            data = new UserAuditData(dbContext);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAudits()
        {
            try
            {
                var record= await data.GetAllAudits();
                return Ok(record);

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
