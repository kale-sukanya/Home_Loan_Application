using CaseStudyFinal.Data;
using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                return await _context.Registers.AnyAsync(r => r.EmailId == emailId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while checking if email exists.", ex);
            }
        }

        public async Task<bool> RegisterUserAsync(Register model)
        {
            try
            {
                var newUser = new Register
                {
                    EmailId = model.EmailId,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password
                };

                _context.Registers.Add(newUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while registering user.", ex);
            }
        }

        public Register GetUserByEmailAndPassword(string emailId, string password, string role)
        {
            try
            {
                return _context.Registers.FirstOrDefault(r => r.EmailId == emailId && r.Password == password && r.Role == role);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while getting user by email and password.", ex);
            }
        }

        public Register GetUserByEmail(string emailId)
        {
            try
            {
                return _context.Registers.FirstOrDefault(u => u.EmailId == emailId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while getting user by email.", ex);
            }
        }

        public Register GetUserByPhone(string phone)
        {
            try
            {
                return _context.Registers.FirstOrDefault(u => u.PhoneNumber == phone);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while getting user by phone number.", ex);
            }
        }

        public async Task UpdatePasswordAsync(Register user, string newPassword)
        {
            try
            {
                user.Password = newPassword;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating user password.", ex);
            }
        }
    }
}
