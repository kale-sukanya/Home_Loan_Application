using Microsoft.AspNetCore.Mvc;

namespace CaseStudyFinal.Controllers
{
    [ApiController]
    [Route("calculator")]
    public class CalculatorController : ControllerBase
    {
        private const double InterestRate = 8.5; 

        [HttpGet("Eligibility")]
        public IActionResult GetEligibility(decimal netMonthlySalary)
        {

            if (netMonthlySalary < 0)
            {
                return BadRequest("Net monthly salary must be non-negative.");
            }

  
            decimal EligibleloanAmount = 60 * (decimal)(0.6 * (double)netMonthlySalary);


            return Ok($"Loan Amount you are eligible for is {EligibleloanAmount.ToString("0.##")}");
        }

        [HttpGet("EMI")]
        public IActionResult GetEMI(double loanAmount, int loanTenureInMonths)
        {

            if (loanAmount <= 0 || loanTenureInMonths <= 0)
            {
                return BadRequest("Invalid loan parameters");
            }

            const double annualInterestRate = 8.5 / 100; 
            const int monthsInYear = 12;

            double monthlyInterestRate = annualInterestRate / monthsInYear;

            double powerTerm = Math.Pow(1 + monthlyInterestRate, loanTenureInMonths);

            double denominator = powerTerm - 1;

            double emi = loanAmount * monthlyInterestRate * powerTerm / denominator;

            return Ok($"Your monthly EMI will be {emi.ToString("0.##")}.");
        }
    }

}
