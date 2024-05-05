using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudyFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocuments _documents;

        public DocumentsController(IDocuments documents)
        {
            _documents = documents;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            var docs = await _documents.GetAll();
            return Ok(docs);
        }


        //Post: {baseurl}/api/documents
        [HttpPost]
        public async Task<IActionResult> UploadDocs([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string ApplicationId)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                var doc = new Documents
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    DateUploaded = DateTime.Now,
                    ApplicationId = ApplicationId
                };

                await _documents.Upload(file, doc);


                return Ok(doc);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[]
            {
                ".pdf"
            };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if (file.Length > 512000)
            {
                ModelState.AddModelError("file", "File size cannot be more than 5MB.");
            }
        }

        [HttpGet]
        [Route("{applicationId}")]
        public async Task<IActionResult> GetDocumentsByApplicationId(string applicationId)
        {
            var docs = await _documents.GetDocumentsByApplicationId(applicationId);
            if (docs == null || !docs.Any())
            {
                return NotFound();
            }

            return Ok(docs);
        }

    }
}