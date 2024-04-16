using CaseStudyFinal.Data;
using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudyFinal.Repositories
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly CaseStudyFinalContext _context;

        public RegisterRepository(CaseStudyFinalContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExistsAsync(string emailId)
        {
            return await _context.Registers.AnyAsync(r => r.EmailId == emailId);
        }

        public async Task<bool> RegisterUserAsync(Register model)
        {
            var newUser = new Register
            {
                EmailId = model.EmailId,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                //Role = "User"
            };

            // Add the new user to the database
            _context.Registers.Add(newUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public Register GetUserByEmailAndPassword(string emailId, string password, string role)
        {
            return _context.Registers.FirstOrDefault(r => r.EmailId == emailId && r.Password == password && r.Role == role);
        }

        public Register GetUserByEmail(string emailId)
        {
            return _context.Registers.FirstOrDefault(u => u.EmailId == emailId);
        }

        public Register GetUserByPhone(string phone)
        {
            return _context.Registers.FirstOrDefault(u => u.PhoneNumber == phone);
        }

        public async Task UpdatePasswordAsync(Register user, string newPassword)
        {
            user.Password = newPassword;
            await _context.SaveChangesAsync();
        }
    }
}
