using LoanApplicationAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LoanApplicationAPI.Services
{
    public class SystemCodeData : ISystemCodeData
    {
        private readonly ApplicationDbContext _context;
        public SystemCodeData(ApplicationDbContext db)
        {
            _context = db;
        }

        public async Task<List<SystemCode>> GetSystemCodes()
        {
            var record = await _context.SystemCode.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).ToListAsync();
            return record;
        }

        public async Task<SystemCode> GetSystemCode(int Id)
        {
            var record = await _context.SystemCode.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).FirstOrDefaultAsync(f => f.Id == Id);
            return record;
        }

        public async Task CreateSystemCode(SystemCode obj, string userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {


                obj.CreatedById = userId;


                _context.SystemCode.Add(obj);
                await _context.SaveChangesAsync();


                string json = JsonSerializer.Serialize(obj);

                UserAudit audit = new UserAudit
                {
                    CreatedById = userId,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = "System Code Add",
                    TransactionData = json
                };

                _context.Audit.Add(audit);
                await _context.SaveChangesAsync();


                await transaction.CommitAsync();
            }
            catch (Exception)
            {

                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateSystemCode(SystemCode obj, string userId, int Id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var record = await _context.SystemCode.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).FirstOrDefaultAsync(f => f.Id == Id);
                if (record != null)
                {
                    record.OrderNo = obj.OrderNo;
                    record.Description = obj.Description;
                    record.Code = obj.Code;
                    record.ModifiedOn = DateTime.UtcNow;
                    record.ModifiedUserId = userId;

                    _context.SystemCode.Update(record);
                    await _context.SaveChangesAsync();

                    string json = JsonSerializer.Serialize(obj);
                    UserAudit audit = new UserAudit
                    {
                        CreatedById = userId,
                        TransactionData = json,
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = "System Code Update",

                    };
                    _context.Audit.Add(audit);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteSystemCode(string userId, int Id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var record = await _context.SystemCode.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).FirstOrDefaultAsync(f => f.Id == Id);
                if (record != null)
                {
                    string json = JsonSerializer.Serialize(record);

                    _context.SystemCode.Remove(record);

                    var audit = new UserAudit
                    {
                        CreatedById = userId,
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = "System Code Delete",
                        TransactionData = json
                    };

                    _context.Audit.Add(audit);
                    await _context.SaveChangesAsync();

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

