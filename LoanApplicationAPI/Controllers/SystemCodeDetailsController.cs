using LoanApplicationAPI.Models;
using LoanApplicationAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace LoanApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemCodeDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly ISystemCodeData systemCodeData;

        public SystemCodeDetailsController(ApplicationDbContext db)
        {
            this.db = db;
            systemCodeData = new SystemCodeData(db);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetSystemCodeDetails()
        {
            var record = await db.SystemCodeDetails.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).ToListAsync();
            return Ok(record);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet(template:"{id}")]
        public async Task<IActionResult> GetSystemCodeDetail(int Id)
        {
            var record = await db.SystemCodeDetails.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).FirstOrDefaultAsync(f => f.Id == Id);
            return Ok(record);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSystemCodeDetails([FromBody]SystemCodeDetails obj)
        {
            await using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                var userId =  User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                obj.CreatedById = userId;
                obj.CreatedOn = DateTime.UtcNow;

                db.SystemCodeDetails.Add(obj);
                await db.SaveChangesAsync();


                string json = JsonSerializer.Serialize(obj);

                UserAudit audit = new UserAudit
                {
                    CreatedById = userId,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = "System Code Details Add",
                    TransactionData = json
                };

                db.Audit.Add(audit);
                await db.SaveChangesAsync();


                await transaction.CommitAsync();

            }
            catch (Exception)
            {

                await transaction.RollbackAsync();
                throw;
            }
            return Ok(obj);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut(template:"{id}")]
        public async Task<IActionResult> UpdateSystemCodeDetails(SystemCodeDetails obj)
        {
            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var record = await db.SystemCodeDetails
                    .FirstOrDefaultAsync(f => f.Id == obj.Id);

                if (record != null)
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    record.Code = obj.Code;
                    record.Description = obj.Description;
                    record.OrderNo = obj.OrderNo;
                    record.SystemCodeId = obj.SystemCodeId;

                    record.ModifiedOn = DateTime.UtcNow;
                    record.ModifiedUserId = userId;

                    db.SystemCodeDetails.Update(record);
                    await db.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            return Ok(obj);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete(template:"{id}")]
        public async Task<IActionResult> DeleteSystemCodeDetails(int Id)
        {
            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var record = await db.SystemCodeDetails.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).FirstOrDefaultAsync(f => f.Id == Id);
                if (record != null)
                {
                    string json = JsonSerializer.Serialize(record);
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    db.SystemCodeDetails.Remove(record);

                    var audit = new UserAudit
                    {

                        CreatedById = userId,
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = "System Code Details Delete",
                        TransactionData = json
                    };

                    db.Audit.Add(audit);
                    await db.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;

            }
            return Ok();
        }

    }
}
