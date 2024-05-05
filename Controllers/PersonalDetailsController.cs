using CaseStudyFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CaseStudyFinal.Interface;

namespace OnlineHomeLoan.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class PersonalDetailsController : ControllerBase
    {
        private readonly IPersonalDetailsRepository _personalDetailsRepository;

        public PersonalDetailsController(IPersonalDetailsRepository repository) 
        {
            _personalDetailsRepository = repository;
        }

        [HttpGet("GetAllDetails")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PersonalDetails>>> GetPersonalDetails()
        {
            return await _personalDetailsRepository.GetAllPersonalDetails();
        }

        [HttpGet("GetDetailsById/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<PersonalDetails>> GetPersonalDetails(string id)
        {
            var personalDetails = await _personalDetailsRepository.GetPersonalDetailsById(id);

            if (personalDetails == null)
            {
                return NotFound();
            }

            return Ok(personalDetails);
          
        }


        [HttpPost("PostPersonalDetails")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<PersonalDetails>> PostPersonalDetails(PersonalDetails personalDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Validation Error");
            }

            var newPersonalDetails = await _personalDetailsRepository.CreatePersonalDetails(personalDetails);


            return Ok(new
            {
                msg = "Submitted"
            });
        }

    }

}
