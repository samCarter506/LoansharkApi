using LoanApplicationAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LoanApplicationAPI.Services
{
    public class SystemCodeDetailsData : ISystemCodeDetailsData
    {

        private readonly ApplicationDbContext _context;
        private readonly ISystemCodeData systemCodeData;

        public SystemCodeDetailsData(ApplicationDbContext db)
        {
            _context = db;
            systemCodeData = new SystemCodeData(db);
        }

        public async Task<List<SystemCodeDetails>> GetSystemCodeDetails()
        {
            var record = await _context.SystemCodeDetails.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).ToListAsync();
            var systemCodeRec = await systemCodeData.GetSystemCode(record.FirstOrDefault().SystemCodeId);
            SystemCodeDetails systemCodeDetailRec = record.FirstOrDefault();

            //record.FirstOrDefault().
            return record;
        }

        public async Task<SystemCodeDetails> GetSystemCodeDetail(int Id)
        {
            var record = await _context.SystemCodeDetails.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).FirstOrDefaultAsync(f => f.Id == Id);
            return record;
        }

        public async Task CreateSystemCodeDetails(SystemCodeDetails obj, string userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {


                obj.CreatedById = userId;


                _context.SystemCodeDetails.Add(obj);
                await _context.SaveChangesAsync();


                string json = JsonSerializer.Serialize(obj);

                UserAudit audit = new UserAudit
                {
                    CreatedById = userId,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = "System Code Details Add",
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

        public async Task UpdateSystemCodeDetails(SystemCodeDetails obj, string userId, int Id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var record = await _context.SystemCodeDetails
                    .FirstOrDefaultAsync(f => f.Id == Id);

                if (record != null)
                {
                    record.Code = obj.Code;
                    record.Description = obj.Description;
                    record.OrderNo = obj.OrderNo;
                    record.SystemCodeId = obj.SystemCodeId; 

                    record.ModifiedOn = DateTime.UtcNow;
                    record.ModifiedUserId = userId;

                    _context.SystemCodeDetails.Update(record);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task DeleteSystemCodeDetails(string userId, int Id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var record = await _context.SystemCodeDetails.Include(f => f.CreatedUser).Include(f => f.ModifiedUser).FirstOrDefaultAsync(f => f.Id == Id);
                if (record != null)
                {
                    string json = JsonSerializer.Serialize(record);

                    _context.SystemCodeDetails.Remove(record);

                    var audit = new UserAudit
                    {
                        CreatedById = userId,
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = "System Code Details Delete",
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
