using CaseStudyFinal.Data;
using CaseStudyFinal.Models;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


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
                mailMessage.Subject = "Home Loan Application Submitted Successfully";
                mailMessage.Body = $"Thank you for registering. Here are your credentials:\n\nEmail: {emailId}\nPhone Number: {phoneNumber}\nPassword: {password}";

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

        public async Task SendApplicationApprovedEmail(/*string aadharNo*/ string trackerId, string email, string applId)
        {
            /*var applicationTracking = await _context.ApplicationTracking
        .Include(at => at.PersonalDetails)
        .FirstOrDefaultAsync(at => at.AadharNo == aadharNo);

            if (applicationTracking == null || applicationTracking.PersonalDetails == null)
            {
                Console.WriteLine("Application tracking record or personal details not found.");
                return;
            }*/

            /*var emailId = applicationTracking.PersonalDetails.EmailId;*/

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
                mailMessage.Subject = "Home Loan Application was approved";
                mailMessage.Body = $"Your Application has been Approved!. The approved Loan Amount has been credited to the account given : \n ApplicationID: {applId}\nTrackingID: {trackerId}\nAccount Number: {accountNumber}.\n\n Regards,\nTeam HL Bank";

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine("Application Approved email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send application submitted email: {ex.Message}");
                }
            }
        }

        public async Task SendApplicationRejectedEmail(string aadharNo)
        {
            var applicationTracking = await _context.ApplicationTracking
        .Include(at => at.PersonalDetails)
        .FirstOrDefaultAsync(at => at.AadharNo == aadharNo);

            if (applicationTracking == null || applicationTracking.PersonalDetails == null)
            {
                Console.WriteLine("Application tracking record or personal details not found.");
                return;
            }

            var emailId = applicationTracking.PersonalDetails.EmailId;

            // Fetch account number based on trackerId
            //var account = await _context.Accounts.FirstOrDefaultAsync(acc => acc.TrackerID == applicationTracking.TrackerID);

            //if (account == null || string.IsNullOrEmpty(account.AccountNumber))
            //{
            //    Console.WriteLine("Account not found or account number is null or empty.");
            //    return;
            //}

            //var accountNumber = account.AccountNumber;

            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Host = "smtp.gmail.com"; // Replace with your SMTP server
                smtpClient.Port = 587; // Port number depends on your SMTP server settings
                smtpClient.EnableSsl = true; // Enable SSL/TLS encryption
                smtpClient.Credentials = new NetworkCredential("sukanyakale2@gmail.com", "nlps pctd xzlc aorm");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("sukanyakale2@gmail.com");
                mailMessage.To.Add((MailAddress)new MailboxAddress("", applicationTracking.PersonalDetails.EmailId)); // Guest's email address
                mailMessage.Subject = "Home Loan Application was Rejected";
                mailMessage.Body = $"We regret to inform you that your application was rejected.\n\n Regards,\nTeam HL Bank";

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
                    Console.WriteLine("Application submitted email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send application submitted email: {ex.Message}");
                }
            }
        }
    }
}
