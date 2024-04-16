using CaseStudyFinal.Models;

namespace CaseStudyFinal.Interface
{
    public interface IPersonalDetailsRepository
    {
        Task<List<PersonalDetails>> GetAllPersonalDetails();
        Task<PersonalDetails> GetPersonalDetailsById(string id);
        Task UpdatePersonalDetails(string id, PersonalDetails personalDetails);
        Task<PersonalDetails> CreatePersonalDetails(PersonalDetails personalDetails);
        Task DeletePersonalDetails(string id);
        bool PersonalDetailsExists(string id);
    }
}
