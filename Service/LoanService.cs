using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;

namespace CaseStudyFinal.Service
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _repository;

        public LoanService(ILoanRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LoanDetails>> GetLoanDetailsAsync()
        {
            return await _repository.GetLoanDetailsAsync();
        }

        public async Task<LoanDetails> GetLoanDetailsByIdAsync(string id)
        {
            return await _repository.GetLoanDetailsByIdAsync(id);
        }

        public async Task UpdateLoanDetailsAsync(string id, LoanDetails loanDetails)
        {
            await _repository.UpdateLoanDetailsAsync(id, loanDetails);
        }

        public async Task AddLoanDetailsAsync(LoanDetails loanDetails)
        {
            await _repository.AddLoanDetailsAsync(loanDetails);
        }

        public async Task DeleteLoanDetailsAsync(string id)
        {
            await _repository.DeleteLoanDetailsAsync(id);
        }
    }
}
