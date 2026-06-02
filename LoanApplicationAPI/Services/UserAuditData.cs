using LoanApplicationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanApplicationAPI.Services
{
    public class UserAuditData : IUserAuditData
    {

        private readonly ApplicationDbContext _context;

        public UserAuditData(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<UserAudit>> GetUserAudits(string userId)
        {
            return await _context.Audit
                .Include(x => x.CreatedUser)
                .Where(x => x.CreatedById == userId || x.ModifiedById == userId)
                .OrderByDescending(x => x.TransactionDate)
                .ToListAsync();
        }


        public async Task<List<UserAudit>> GetAllAudits()
        {
            return await _context.Audit
                .Include(x => x.CreatedUser)
                .OrderByDescending(x => x.TransactionDate)
                .ToListAsync();
        }


        public async Task<UserAudit?> GetAuditById(int id)
        {
            return await _context.Audit
                .Include(x => x.CreatedUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task CreateAudit(UserAudit audit)
        {
     
            audit.TransactionDate = DateTime.UtcNow;

            _context.Audit.Add(audit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAudit(int id)
        {
            var record = await _context.Audit.FindAsync(id);

            if (record == null)
                return;

            _context.Audit.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

}

