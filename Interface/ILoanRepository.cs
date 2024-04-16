using CaseStudyFinal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseStudyFinal.Interface
{
    public interface ILoanRepository
    {
        Task<IEnumerable<LoanDetails>> GetLoanDetailsAsync();
        Task<LoanDetails> GetLoanDetailsByIdAsync(string id);
        Task UpdateLoanDetailsAsync(string id, LoanDetails loanDetails);
        Task AddLoanDetailsAsync(LoanDetails loanDetails);
        Task DeleteLoanDetailsAsync(string id);
        bool LoanDetailsExists(string id);
    }
}
