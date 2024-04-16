using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudyFinal.Controllers
{
    [ApiController]
    [Route("calculator")]
    public class CalculatorController : ControllerBase
    {
        private const double InterestRate = 8.5; /// 12 / 100; // Monthly interest rate

        [HttpGet("Eligibility")]
        public IActionResult GetEligibility(decimal netMonthlySalary)
        {
            // Check if netMonthlySalary is not negative
            if (netMonthlySalary < 0)
            {
                return BadRequest("Net monthly salary must be non-negative.");
            }

            // Calculate the loan amount 
            decimal EligibleloanAmount = 60 * (decimal)(0.6 * (double)netMonthlySalary);

            // Return  loan amount
            return Ok($"Loan Amount you are eligible for is {EligibleloanAmount.ToString("0.##")}");
        }

        [HttpGet("EMI")]
        public IActionResult GetEMI(double loanAmount, int loanTenureInMonths)
        {
            // Check if loan amount and loan tenure are valid
            if (loanAmount <= 0 || loanTenureInMonths <= 0)
            {
                return BadRequest("Invalid loan parameters");
            }

            const double annualInterestRate = 8.5 / 100; // 8.5% annual interest rate
            const int monthsInYear = 12;

            // Convert annual interest rate to monthly rate
            double monthlyInterestRate = annualInterestRate / monthsInYear;

            // Calculate the power term (1 + R)^n
            double powerTerm = Math.Pow(1 + monthlyInterestRate, loanTenureInMonths);

            // Calculate the denominator ((1 + R)^n - 1)
            double denominator = powerTerm - 1;

            // Calculate EMI
            double emi = loanAmount * monthlyInterestRate * powerTerm / denominator;



            // Return the calculated EMI
            return Ok($"Your monthly EMI will be {emi.ToString("0.##")}.");
        }
    }

}
