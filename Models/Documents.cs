using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CaseStudyFinal.Models
{
    public class Documents
    {
        [Key]
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Url { get; set; }
        public DateTime DateUploaded { get; set; }

        [ForeignKey("ApplicationId")]
        [Required]
        public string ApplicationId { get; set; }

        public LoanDetails LoanDetails { get; set; }

    }
}
