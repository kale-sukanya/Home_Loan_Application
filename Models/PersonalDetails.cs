using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CaseStudyFinal.Models
{
    public class PersonalDetails
    {

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middle name is required")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [ForeignKey("EmailId")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of Birth is required")]
        //[RegularExpression(@"^(0[1-9]|1[0-2])[-\/](0[1-9]|[12]\d|3[01])[-\/](19\d{2}|200[0-6])$", ErrorMessage ="Year of birth should be 2006 or less")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Nationality is required")]
        public string Nationality { get; set; }

        [Key]
        [Required(ErrorMessage = "Aadhar number is required")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhar Number must be exactly 12 digits")]
        public string AadharNo { get; set; }

        [Required(ErrorMessage = "PAN number is required")]
        [RegularExpression("^[A-Z]{5}[0-9]{4}[A-Z]$", ErrorMessage = "Invalid PAN number format. PAN number should be in the format: ABCDE1234F")]
        public string PanNo { get; set; }

        [ForeignKey("ApplicationId")]
        [RegularExpression(@"^\d{4}$")]
        public string ApplicationId { get; set; }

        // Navigation property
        public virtual LoanDetails LoanDetails { get; set; }
        public virtual Register Register { get; set; }
    }
}
