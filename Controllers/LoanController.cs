using CaseStudyFinal.Data;
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
        private readonly CaseStudyFinalContext _context;

        public LoanController(ILoanService service, CaseStudyFinalContext context)
        {
            _service = service;
            _context = context;
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
        //[Authorize(Roles = "User")]
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

        [HttpGet("applications/{email}")]
        public IActionResult GetApplicationsByEmail(string email)
        {
            var applicationsData = (from ld in _context.Loans
                                    join pd in _context.PersonalDetails on ld.ApplicationId equals pd.ApplicationId
                                    join d in _context.Documents on ld.ApplicationId equals d.ApplicationId
                                    where pd.EmailId == email
                                    select new
                                    {
                                        LoanDetails = ld,
                                        PersonalDetails = pd,
                                        Document = d
                                    }).ToList();

            var loanDetailsArray = applicationsData.Select(data => data.LoanDetails).Distinct().ToList();
            var personalDetailsArray = applicationsData.Select(data => data.PersonalDetails).Distinct().ToList();

            var documentsArrays = new List<List<Documents>>();
            foreach (var loanApplication in loanDetailsArray)
            {
                var applicationDocuments = applicationsData
                    .Where(data => data.LoanDetails.ApplicationId == loanApplication.ApplicationId)
                    .Select(data => data.Document)
                    .ToList();
                documentsArrays.Add(applicationDocuments);
            }

            var responseData = new
            {
                LoanDetails = loanDetailsArray,
                PersonalDetails = personalDetailsArray,
                Documents = documentsArrays
            };

            return Ok(responseData);
        }
    }

}
