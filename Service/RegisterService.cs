using CaseStudyFinal.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CaseStudyFinal.Interface;
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

            string recipientEmail = model.EmailId; 
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
            if(user != null)
            {
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
            else
            {
                return "User not found";
            }
            
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

        private string GetUserRole(string role)
        {
            if (role == "Admin")
            {
                return _config["Jwt:AdminRole"]; 
            }
            else if (role == "User")
            {
                return _config["Jwt:UserRole"]; 
            }
            else
            {
                return string.Empty; 
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
