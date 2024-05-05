using CaseStudyFinal.DTO;
using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CaseStudyFinal.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> RegisterAsync(Register model)
        {
            var result = await _registerService.RegisterUserAsync(model);
            if (result)
            {
                return Ok(new
                {
                    msg = "User registered successfully"
                });
            }
            else
            {
                return Conflict(new
                {
                    msg = "Email already exists"
                });
            }
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> Login(Register model)
        {
            var result = await _registerService.LoginAsync(model);
            if (result != null)
            {
                if(result == "Check Password and try again")
                {
                    return BadRequest(new
                    {
                        msg = result
                    });
                }
                else if(result == "Invalid Role")
                {
                    return BadRequest(new
                    {
                        msg = result
                    }) ;
                }
                else
                {
                    return Ok(new
                    {
                        email = model.EmailId,
                        role = model.Role,
                        token = result
                    });
                }
                
            }
            else
            {
                return NotFound(new
                {
                    msg = "Invalid email or password or role"
                });
            }
        }

        [HttpPost]
        [Route("/ForgotPassword/{email}")]
        public async Task<IActionResult> Forgotpassword(string email)
        {
            string result = await _registerService.SendOtp(email);
            if(result == "User not found")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPost("/Resetpassword")]

        public async Task<IActionResult> ResetPassword(ResetPassDto request)
        {
            string result = await _registerService.ResetPassword(request);
            if(result == "User not found")
            {
                return BadRequest(result);
            }
            else if(result == "Invalid OTP")
            {
                return BadRequest(result);
            }
            else if(result == "New password is required")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
