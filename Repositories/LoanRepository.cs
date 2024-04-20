using CaseStudyFinal.Data;
using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudyFinal.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly CaseStudyFinalContext _context;

        public LoanRepository(CaseStudyFinalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoanDetails>> GetLoanDetailsAsync()
        {
            return await _context.Loans.ToListAsync();
        }

        public async Task<LoanDetails> GetLoanDetailsByIdAsync(string id)
        {
            return await _context.Loans.FindAsync(id);
        }

        public async Task UpdateLoanDetailsAsync(string id, LoanDetails loanDetails)
        {
            if (id != loanDetails.ApplicationId)
            {
                throw new ArgumentException("ID mismatch");
            }

            _context.Entry(loanDetails).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddLoanDetailsAsync(LoanDetails loanDetails)
        {
            _context.Loans.Add(loanDetails);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLoanDetailsAsync(string id)
        {
            var loanDetails = await _context.Loans.FindAsync(id);
            if (loanDetails != null)
            {
                _context.Loans.Remove(loanDetails);
                await _context.SaveChangesAsync();
            }
        }

        public bool LoanDetailsExists(string id)
        {
            return _context.Loans.Any(e => e.ApplicationId == id);
        }



    }
}
