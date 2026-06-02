using LoanApplicationAPI.Models;
using LoanApplicationAPI.Models.DTO;
using LoanApplicationAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Security.Claims;

namespace LoanApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemCodeController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public SystemCodeController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<List<SystemCode>> GetSystemCodes()
        {
            var record = await db.SystemCode.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).ToListAsync();
            return record;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<SystemCode> GetSystemCode(int Id)
        {
            var record = await db.SystemCode.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).FirstOrDefaultAsync(f => f.Id == Id);
            return record;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSystemCode([FromBody] SystemCode obj)
        {
            await using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                var exists = await db.SystemCode
                                    .AnyAsync(x =>
                                     x.Code.ToLower() == obj.Code.ToLower());

                if (exists)
                {
                    return BadRequest(
                        "System code already exists.");
                }
                var userId =
                 User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                obj.CreatedById = userId;
                obj.CreatedOn = DateTime.UtcNow;

                db.SystemCode.Add(obj);
                await db.SaveChangesAsync();


                string json = JsonSerializer.Serialize(obj);

                UserAudit audit = new UserAudit
                {
                    CreatedById = userId,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = "System Code Add",
                    TransactionData = json
                };

                db.Audit.Add(audit);
                await db.SaveChangesAsync();


                await transaction.CommitAsync();

                return Ok(obj);
            }
            catch (Exception)
            {

                await transaction.RollbackAsync();
                throw;
            }

            
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSystemCode(int id,[FromBody] SystemCode obj)
        {
            using var transaction =
                await db.Database.BeginTransactionAsync();

            try
            {
                var record = await db.SystemCode
                    .Include(f => f.CreatedUser)
                    .Include(f => f.ModifiedUser)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (record == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier
                )?.Value;

                record.OrderNo = obj.OrderNo;
                record.Description = obj.Description;
                record.Code = obj.Code;
                record.ModifiedOn = DateTime.UtcNow;
                record.ModifiedUserId = userId;

                await db.SaveChangesAsync();

                string json = JsonSerializer.Serialize(record);

                UserAudit audit = new UserAudit
                {
                    CreatedById = userId,
                    TransactionData = json,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = "System Code Update",
                };

                db.Audit.Add(audit);

                await db.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(record);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task DeleteSystemCode( int Id)
        {
            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var record = await db.SystemCode.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).FirstOrDefaultAsync(f => f.Id == Id);
                if (record != null)
                {
                    string json = JsonSerializer.Serialize(record);
                    var userId = User.FindFirst("UserId")?.Value;
                    db.SystemCode.Remove(record);

                    var audit = new UserAudit
                    {
                        CreatedById = userId,
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = "System Code Delete",
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
        }
    }
}
