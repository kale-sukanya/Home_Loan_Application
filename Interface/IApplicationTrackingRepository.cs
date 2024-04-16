using System.Collections.Generic;
using System.Threading.Tasks;
using CaseStudyFinal.DTO;
using CaseStudyFinal.Models;

namespace CaseStudyFinal.Interface
{
    public interface IApplicationTrackingRepository
    {
        Task<IEnumerable<ApplicationTracking>> GetAllApplications();
        Task<ApplicationTracking> GetApplicationById(string trackerId);//, string applicationId);
        
        //Task UpdateApplicationStatus(string trackerId, string newStatus);
        //Task<ApplicationTracking> UpdateApplicationStatus(ApplicationTracking model);
        //Task<PersonalDetails> UpdatePersonalDetailsAsync(PersonalDetails model);

        //Task<ApplicationTracking> UpdateApplicationTrackingDetailsAsync(string id, ApplicationTracking appDetails);

        Task<string> Update(string trackerId, UpdateStatusDto updateStatus, string email);
    }
}
