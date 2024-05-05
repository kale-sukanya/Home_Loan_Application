﻿using CaseStudyFinal.Models;

namespace CaseStudyFinal.Interface
{
    public interface ILoanService
    {
        Task<IEnumerable<LoanDetails>> GetLoanDetailsAsync();
        Task<LoanDetails> GetLoanDetailsByIdAsync(string id);
        Task UpdateLoanDetailsAsync(string id, LoanDetails loanDetails);
        Task AddLoanDetailsAsync(LoanDetails loanDetails);
        Task DeleteLoanDetailsAsync(string id);
    }
}
