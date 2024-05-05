using CaseStudyFinal.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudyFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet("GetAccountBalance/{accountNo}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAccountBalance([FromRoute]string accountNo)
        {
            var account = await _accountRepository.GetAccountByAccountNumberAsync(accountNo);
            if (account == null || accountNo != account.AccountNumber)
            {
                return NotFound(new
                {
                    error = "Account Number not found"
                });
            }
            return Ok(account);
        }
    }
}
