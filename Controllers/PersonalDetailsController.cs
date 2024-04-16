using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseStudyFinal.Models;
using CaseStudyFinal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaseStudyFinal.Service;
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
        //[Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<PersonalDetails>> GetPersonalDetails(string id)
        {
            var personalDetails = await _personalDetailsRepository.GetPersonalDetailsById(id);

            if (personalDetails == null)
            {
                return NotFound();
            }

            return Ok(personalDetails);
          
        }

        [HttpPut("UpdateDetails/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PutPersonalDetails(string id, PersonalDetails personalDetails)
        {
            try
            {
                await _personalDetailsRepository.UpdatePersonalDetails(id, personalDetails);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    errorMessage = ex.Message
                });
            }
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

        [HttpDelete("DeletePersonalDetails/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeletePersonalDetails(string id)
        {
            await _personalDetailsRepository.DeletePersonalDetails(id);
            return NoContent();
        }
    }

}
