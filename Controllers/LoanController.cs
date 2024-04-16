using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;
using CaseStudyFinal.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseStudyFinal.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _service;

        public LoanController(ILoanService service)
        {
            _service = service;
        }

        [HttpGet("/AdminGetAllLoanDetails")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<LoanDetails>>> GetLoanDetails()
        {
            var loanDetails = await _service.GetLoanDetailsAsync();
            return Ok(new
            {
                result = loanDetails
            });
        }

        [HttpGet("/userGetLoanDetails/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<LoanDetails>> GetLoanDetails(string id)
        {
            var loanDetails = await _service.GetLoanDetailsByIdAsync(id);

            if (loanDetails == null)
            {
                return NotFound();
            }

            return loanDetails;
        }

        [HttpPut("/userUpdate/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PutLoanDetails(string id, LoanDetails loanDetails)
        {
            await _service.UpdateLoanDetailsAsync(id, loanDetails);
            return Ok(new
            {
                result = loanDetails
            });
        }

        [HttpPost("/userPostLoanDetails")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<LoanDetails>> PostLoanDetails(LoanDetails loanDetails)
        {

            await _service.AddLoanDetailsAsync(loanDetails);
            return Ok(new
            {
                msg = "Loan Details Submitted Successfully"
            });
        }

        [HttpDelete("/userDeleteApplication/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteLoanDetails(string id)
        {
            await _service.DeleteLoanDetailsAsync(id);
            return Ok(new
            {
                msg = "Loan Details Deleted Successfully"
            });
        }
    }
}
