
using CaseStudyFinal.Data;
using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseStudyFinal.Repositories
{
    public class DocumentsRepository: IDocuments
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CaseStudyFinalContext _context;

        public DocumentsRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            CaseStudyFinalContext context)
        {
            this._webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IEnumerable<Documents>> GetAll()
        {
            return await _context.Documents.ToListAsync();
        }

        public async Task<IEnumerable<Documents>> GetDocumentsByApplicationId(string applicationId)
        {
            var docs = await _context.Documents
            .Where(doc => doc.ApplicationId == applicationId)
            .ToListAsync();
            return docs;

            
        }

        public async Task<Documents> Upload(IFormFile file, Documents doc)
        {
            //this is my storage path 
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath,
                "Documents", $"{doc.FileName}{doc.FileExtension}");

            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var httpRequest = httpContextAccessor.HttpContext.Request;

            //this is my url path
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Documents/{doc.FileName}{doc.FileExtension}";

            doc.Url = urlPath;

            await _context.Documents.AddAsync(doc);
            await _context.SaveChangesAsync();

            return doc;
        }
    }
}
