using CaseStudyFinal.Models;

namespace CaseStudyFinal.Interface
{
    public interface IDocuments
    {
        Task<Documents> Upload(IFormFile file, Documents doc);

        Task<IEnumerable<Documents>> GetAll();

        Task<IEnumerable<Documents>> GetDocumentsByApplicationId(string applicationId);
    }
}
