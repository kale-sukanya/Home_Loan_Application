using CaseStudyFinal.DTO;
using CaseStudyFinal.Models;
using System.Threading.Tasks;

namespace CaseStudyFinal.Interface
{
    public interface IRegisterService
    {
        Task<bool> RegisterUserAsync(Register model);
        Task<string> LoginAsync(Register model);
        Task<string> SendOtp(string emailId);
        Task<string> ResetPassword(ResetPassDto request);
    }
}
