namespace CaseStudyFinal.Models
{
    public class LoginDto
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User";
    }
}
