using CaseStudyFinal.Data;
using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseStudyFinal.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CaseStudyFinalContext _context;

        public AccountRepository(CaseStudyFinalContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAccountByAccountNumberAsync(string accountNumber)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }
    }
}
