using CaseStudyFinal.DTO;
using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudyFinal.Controllers
{
    [Route("/[controller]")]
    public class ApplicationTrackingController : ControllerBase
    {
        private readonly IApplicationTrackingRepository _repository;

        public ApplicationTrackingController(IApplicationTrackingRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("/AdminGetAllApplicationStatus")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ApplicationTracking>>> GetAllApplicationsStatus()
        {
            var applications = await _repository.GetAllApplications();
            return Ok(applications);
        }


        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetApplicationStatus([FromRoute] string id)
        {
            var application = await _repository.GetApplicationById(id);
            if (application == null || id != application.TrackerID)
            {
                return NotFound(new
                {
                    error = "Application not found"
                });
            }


            return Ok(new
            {
                TrackerID = application.TrackerID,
                ApplicationID = application.ApplicationID,
                Status = application.Status,
                AadharNo = application.AadharNo,
            });
        }


        [HttpPut]
        [Route("UpdateStatus/{trackerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatusAsync(string trackerId, [FromBody] UpdateStatusDto model, string email)
        {
            try
            {
                string result = await _repository.Update(trackerId,model, email);
                if(result == "updated")
                {
                    return Ok(result); 
                }
                return NotFound(new
                {
                    error = "Not found"
                });
               
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new
                {
                    errorMessage = ex.Message
                });
            }
        }
    }
}
