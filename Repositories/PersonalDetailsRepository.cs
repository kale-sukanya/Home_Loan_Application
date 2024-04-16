using CaseStudyFinal.Data;
using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;
using CaseStudyFinal.Service;
using Microsoft.EntityFrameworkCore;

namespace CaseStudyFinal.Repositories
{
    public class PersonalDetailsRepository : IPersonalDetailsRepository
    {
        private readonly CaseStudyFinalContext _context;
        private readonly EmailService _emailService;

        public PersonalDetailsRepository(CaseStudyFinalContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<List<PersonalDetails>> GetAllPersonalDetails()
        {
            return await _context.PersonalDetails.ToListAsync();
        }

        public async Task<PersonalDetails> GetPersonalDetailsById(string id)
        {
            var appl = await _context.PersonalDetails.FirstOrDefaultAsync(x=>x.ApplicationId == id);
            if (appl == null)
            {
                return null;
            }
            return appl;
        }

        public async Task UpdatePersonalDetails(string id, PersonalDetails personalDetails)
        {
            if (id != personalDetails.AadharNo)
            {
                throw new ArgumentException("ID mismatch");
            }

            _context.Entry(personalDetails).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<PersonalDetails> CreatePersonalDetails(PersonalDetails personalDetails)
        {
            var appId = await _context.Loans.FindAsync(personalDetails.ApplicationId);
            if (appId == null)
            {
                throw new ArgumentException("Application ID should not be null");
            }
            personalDetails.LoanDetails = appId;

            var email = _context.Registers.Find(personalDetails.EmailId);
            if (email == null)
            {
                throw new ArgumentException("Invalid Email");
            }
            personalDetails.Register = email;
            _context.PersonalDetails.Add(personalDetails);
            await _context.SaveChangesAsync();

            string trackingId = GeneratetrackingId();

            // Set the status of the application to "pending"
            string status = "pending";

            // Create a new ApplicationTracking instance
            var applicationTracking = new ApplicationTracking
            {
                TrackerID = trackingId,
                ApplicationID = personalDetails.ApplicationId, // Assuming ApplicationId is present in PersonalDetails
                Status = status,
                AadharNo = personalDetails.AadharNo // Assuming PhoneNumber is present in PersonalDetails
                                                    // Add other properties as needed
            };

            // Add the new ApplicationTracking record to the context
            _context.ApplicationTracking.Add(applicationTracking);
            await _context.SaveChangesAsync();

            string recipientEmail = personalDetails.EmailId; // Get recipient email from the submitted form or database
            await _emailService.SendApplicationSentEmail(recipientEmail, applicationTracking.ApplicationID, applicationTracking.TrackerID);

            return personalDetails;
        }

        public async Task DeletePersonalDetails(string id)
        {
            var personalDetails = await _context.PersonalDetails.FindAsync(id);
            if (personalDetails != null)
            {
                _context.PersonalDetails.Remove(personalDetails);
                await _context.SaveChangesAsync();
            }
        }

        public bool PersonalDetailsExists(string id)
        {
            return _context.PersonalDetails.Any(e => e.AadharNo == id);
        }

        private string GeneratetrackingId()
        {
            // You can implement your own logic to generate account numbers here
            // For simplicity, let's assume it generates a random 10-digit number
            Random random = new Random();
            long accountNumber = random.Next(1000, 9999);
            return accountNumber.ToString();
        }
    }

}
