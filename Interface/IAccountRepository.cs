using CaseStudyFinal.Data;
using CaseStudyFinal.Models;
using System.Threading.Tasks;

namespace CaseStudyFinal.Interface
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByAccountNumberAsync(string accountNumber);
    }
}
