using CaseStudyFinal.Models;

namespace CaseStudyFinal.Interface
{
    public interface IRegisterRepository
    {
        Task<bool> EmailExistsAsync(string emailId);
        Task<bool> RegisterUserAsync(Register model);
        Register GetUserByEmailAndPassword(string emailId, string password, string role);
        Register GetUserByEmail(string emailId);
        Register GetUserByPhone(string phone);
        Task UpdatePasswordAsync(Register user, string newPassword);
    }
}
