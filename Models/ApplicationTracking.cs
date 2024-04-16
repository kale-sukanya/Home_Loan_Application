using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CaseStudyFinal.Models
{
    public class ApplicationTracking
    {
        [Key]
        public string TrackerID { get; set; }

        [Required]
        [ForeignKey("ApplicationId")]
        public string ApplicationID { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        [ForeignKey("AadharNo")]
        public string AadharNo { get; set; }


        public LoanDetails LoanDetails { get; set; }

        public PersonalDetails PersonalDetails { get; set; }

    }
}
