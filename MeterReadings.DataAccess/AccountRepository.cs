using MeterReadings.Domain;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.DataAccess
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountContext _context;

        public AccountRepository(AccountContext accountContext)
        {
            _context = accountContext;
        }

        public async Task<ICollection<Account>> Get(IEnumerable<int> accountIds)
        {
            return await _context.Accounts
                .Where(x => accountIds.Contains(x.AccountId))
                .Include(x => x.MeterReadings)
                .ToListAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
