using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CaseStudyFinal.Models
{
    public class Account
    {
        [Key]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Tracker ID is required")]
        [ForeignKey("TrackerID")]
        public string TrackerID { get; set; }

        [Required(ErrorMessage = "Balance is required")]
        public decimal Balance { get; set; }

        // Navigation property

        public ApplicationTracking ApplicationTracking { get; set; }
    }

}
