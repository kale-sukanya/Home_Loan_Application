using CaseStudyFinal.Data;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Net;
using System.Net.Mail;


namespace CaseStudyFinal.Service
{

    public class EmailService
    {
        private readonly CaseStudyFinalContext _context;
        public EmailService(CaseStudyFinalContext context)
        {
            _context = context;
        }
        public async Task SendReistrationSuccessEmail(string recipientEmail, string emailId, string phoneNumber, string password)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Host = "smtp.gmail.com"; // Replace with your SMTP server
                smtpClient.Port = 587; // Port number depends on your SMTP server settings
                smtpClient.EnableSsl = true; // Enable SSL/TLS encryption
                smtpClient.Credentials = new NetworkCredential("sukanyakale2@gmail.com", "nlps pctd xzlc aorm");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("sukanyakale2@gmail.com");
                mailMessage.To.Add(recipientEmail);
                mailMessage.Subject = "User Registered Successfully";
                mailMessage.Body = $"Thank you for registering. Here are your credentials:\n\nEmail: {emailId}\nPhone Number: {phoneNumber}\nPassword: {password}";

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine("Registration email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send registration email: {ex.Message}");
                }
            }
        }

        public async Task SendApplicationSentEmail(string recipientEmail, string applicationId, string trackingId)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Host = "smtp.gmail.com"; // Replace with your SMTP server
                smtpClient.Port = 587; // Port number depends on your SMTP server settings
                smtpClient.EnableSsl = true; // Enable SSL/TLS encryption
                smtpClient.Credentials = new NetworkCredential("sukanyakale2@gmail.com", "nlps pctd xzlc aorm");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("sukanyakale2@gmail.com");
                mailMessage.To.Add(recipientEmail);
                mailMessage.Subject = "Home Loan Application Submitted Successfully";
                mailMessage.Body = $"Your Application has been sent. Find your ApplicationID and TrackingID below:\nApplicationID: {applicationId}\nTrackingID: {trackingId}";

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine("Application submitted email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send application submitted email: {ex.Message}");
                }
            }
        }

        public async Task SendApplicationStatusEmail(string trackerId, string email, string subject, string body)
        {
            // Fetch account number based on trackerId
            var account = await _context.Accounts.FirstOrDefaultAsync(acc => acc.TrackerID == trackerId);

            if (account == null || string.IsNullOrEmpty(account.AccountNumber))
            {
                Console.WriteLine("Account not found or account number is null or empty.");
                return;
            }

            var accountNumber = account.AccountNumber;

            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Host = "smtp.gmail.com"; // Replace with your SMTP server
                smtpClient.Port = 587; // Port number depends on your SMTP server settings
                smtpClient.EnableSsl = true; // Enable SSL/TLS encryption
                smtpClient.Credentials = new NetworkCredential("sukanyakale2@gmail.com", "nlps pctd xzlc aorm");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("sukanyakale2@gmail.com");
                mailMessage.To.Add((MailAddress)new MailboxAddress("",email)); // Guest's email address
                mailMessage.Subject = subject;
                mailMessage.Body = body /**/;

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine("Application Staus email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send status email: {ex.Message}");
                }
            }
        }


        public async Task SendEmailOTP(string recipientEmail, string body, string subject)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Host = "smtp.gmail.com"; // Replace with your SMTP server
                smtpClient.Port = 587; // Port number depends on your SMTP server settings
                smtpClient.EnableSsl = true; // Enable SSL/TLS encryption
                smtpClient.Credentials = new NetworkCredential("sukanyakale2@gmail.com", "nlps pctd xzlc aorm");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("sukanyakale2@gmail.com");
                mailMessage.To.Add(recipientEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine("OTP Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send OTP Email: {ex.Message}");
                }
            }
        }
    }
}
