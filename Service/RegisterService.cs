using CaseStudyFinal.Models;
using CaseStudyFinal.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CaseStudyFinal.Service;
using CaseStudyFinal.Interface;
using static System.Net.WebRequestMethods;
using Org.BouncyCastle.Asn1.Ocsp;
using CaseStudyFinal.DTO;

namespace CaseStudyFinal.Service
{
    public class RegisterService : IRegisterService
    {
        private readonly IRegisterRepository _repository;
        private readonly IConfiguration _config;
        private readonly EmailService _emailService;
        private static readonly Dictionary<string, string> otpStorage = new Dictionary<string, string>();

        public RegisterService(IRegisterRepository repository, IConfiguration config, EmailService emailService)
        {
            _repository = repository;
            _config = config;
            _emailService = emailService;
        }

        public async Task<bool> RegisterUserAsync(Register model)
        {
            if (await _repository.EmailExistsAsync(model.EmailId))
            {
                return false;
            }

            await _repository.RegisterUserAsync(model);

            string recipientEmail = model.EmailId; // Get recipient email from the submitted form or database
            await _emailService.SendReistrationSuccessEmail(recipientEmail, model.EmailId, model.PhoneNumber, model.Password);

            return true;
        }

        public async Task<string> LoginAsync(Register model)
        {
            var user = _repository.GetUserByEmailAndPassword(model.EmailId, model.Password, model.Role);

            if (user != null)
            {
                var jwt = _config.GetSection("Jwt").Get<Jwt>();
                var userRole = GetUserRole(user.Role);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(ClaimTypes.Role, userRole),
                    new Claim("EmailId", user.EmailId),
                    new Claim("Password", user.Password)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: signIn
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            return null;
        }

        public async Task<string> SendOtp(string emailId)
        {
            var user = _repository.GetUserByEmail(emailId);
            string otp = GenerateOTP();
            otpStorage[emailId] = otp;
            try
            {
                string subject = "OTP for password reset";
                string Body = $"Your OTP for password reset is: {otp}";
                await _emailService.SendEmailOTP(emailId, Body, subject);
            }
            catch (Exception)
            {
                return "Error sending OTP email";
            }

            return "OTP sent successfully";
        }

        public async Task<string> ResetPassword(ResetPassDto request)
        {
            var user = _repository.GetUserByEmail(request.EmailId);
            if (user == null)
            {
                return "User not found";
            }
            string storedOtp = otpStorage[user.EmailId];

            if (storedOtp != request.OTP)
            {
                return "Invalid OTP";
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return "New password is required";
            }

            user.Password = request.Password;
            await _repository.UpdatePasswordAsync(user, request.Password);

            otpStorage.Remove(user.EmailId);

            string subject = "Password Reset Success!";

            string Body = $"Your password has been reset successfully.\nEmail: {user.EmailId}. Your new password is: {request.Password}.";

            await _emailService.SendEmailOTP(user.EmailId, Body, subject);

            return "Password reset successfully";

        }

        public async Task<string> ForgotUserAsync(string phone)
        {
            var user = _repository.GetUserByPhone(phone);
            if (user != null)
            {
                return "Your emailId is " + user.EmailId;
            }

            return "User Not found. Please register.";
        }

        private string GetUserRole(string userType)
        {
            if (userType == "Admin")
            {
                return _config["Jwt:AdminRole"]; // Get Admin role from configuration
            }
            else if (userType == "User")
            {
                return _config["Jwt:UserRole"]; // Get User role from configuration
            }
            else
            {
                return string.Empty; // Default role
            }
        }

        private string GenerateOTP()
        {
            // Generate a random 6-digit OTP
            Random rnd = new Random();
            string otpNumber = rnd.Next(100000, 999999).ToString();
            return otpNumber;
        }


    }
}
