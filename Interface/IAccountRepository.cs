using CaseStudyFinal.Models;

namespace CaseStudyFinal.Interface
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByAccountNumberAsync(string accountNumber);
    }
}
