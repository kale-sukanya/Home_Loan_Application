using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseStudyFinal.Models
{
    public class LoanDetails
    {

        [Key]
        [RegularExpression(@"^\d{4}$")]
        public string ApplicationId { get; set; }

        [Required(ErrorMessage = "Property location is required")]
        public string PropertyLocation { get; set; }

        [Required(ErrorMessage = "Property name is required")]
        public string PropertyName { get; set; }

        [Required(ErrorMessage = "Estimate amount is required")]
        [RegularExpression(@"^(?!0+(\.0+)?$)([1-9]\d{4,}|[1-9]\d{3,}\.\d{2})$", ErrorMessage = "Estimate amount cannot be less than 10,000.")]
        public int EstimateAmount { get; set; }

        [Column(TypeName = "varchar(10)")]
        [Required(ErrorMessage = "Type of employment is required")]
        public string TypeOfEmployment { get; set; }

        [Required(ErrorMessage = "Retirement age is required")]
        //[RegularExpression(@"^(5\d|[6-9]\d|100)$", ErrorMessage = "Retirement Age cannot be less than 50")]
        public int RetirementAge { get; set; }

        [Required(ErrorMessage = "Organization type is required")]
        public string OrganizationType { get; set; }

        [Required(ErrorMessage = "Employer name is required")]
        public string EmployerName { get; set; }

        [Required(ErrorMessage = "Net salary is required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Net Salary cannot be zero.")]
        public int NetSalary { get; set; }

        [Required(ErrorMessage = "Max Grantable Loan Amount is required")]
        [RegularExpression(@"^(?!0+(\.0+)?$)([1-9]\d{4,}|[1-9]\d{3,}\.\d{2})$", ErrorMessage = "Max loan grantable amount cannot be less than 10,000.")]
        public double MaxLoanAmountGrantable { get; set; }


        [Required(ErrorMessage = "InterestRate is required")]
        public double InterestRate { get; set; } = 8.5;


        [Required(ErrorMessage = "Loan amount is required")]
        [RegularExpression(@"^(?!0+(\.0+)?$)([1-9]\d{4,}|[1-9]\d{3,}\.\d{2})$", ErrorMessage = "Loan amount cannot be less than 10,000.")]
        public int LoanAmount { get; set; }

        [Required(ErrorMessage = "Enter tenure in months")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Tenure cannot be zero.")]
        public int Tenure { get; set; }

    }
}
