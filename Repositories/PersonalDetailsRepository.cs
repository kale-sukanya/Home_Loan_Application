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
            try
            {
                return await _context.PersonalDetails.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching personal details.", ex);
            }
        }

        public async Task<PersonalDetails> GetPersonalDetailsById(string id)
        {
            try
            {
                var appl = await _context.PersonalDetails.FirstOrDefaultAsync(x => x.ApplicationId == id);
                return appl;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching personal details by ID.", ex);
            }
        }

        public async Task UpdatePersonalDetails(string id, PersonalDetails personalDetails)
        {
            try
            {
                if (id != personalDetails.AadharNo)
                {
                    throw new ArgumentException("ID mismatch");
                }

                _context.Entry(personalDetails).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating personal details.", ex);
            }
        }

        public async Task<PersonalDetails> CreatePersonalDetails(PersonalDetails personalDetails)
        {
            try
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

                string status = "pending";

                var applicationTracking = new ApplicationTracking
                {
                    TrackerID = trackingId,
                    ApplicationID = personalDetails.ApplicationId,
                    Status = status,
                    AadharNo = personalDetails.AadharNo
                };

                _context.ApplicationTracking.Add(applicationTracking);
                await _context.SaveChangesAsync();

                string recipientEmail = personalDetails.EmailId;
                await _emailService.SendApplicationSentEmail(recipientEmail, applicationTracking.ApplicationID, applicationTracking.TrackerID);

                return personalDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating personal details.", ex);
            }
        }

        public async Task DeletePersonalDetails(string id)
        {
            try
            {
                var personalDetails = await _context.PersonalDetails.FindAsync(id);
                if (personalDetails != null)
                {
                    _context.PersonalDetails.Remove(personalDetails);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting personal details.", ex);
            }
        }

        public bool PersonalDetailsExists(string id)
        {
            try
            {
                return _context.PersonalDetails.Any(e => e.AadharNo == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while checking if personal details exist.", ex);
            }
        }

        private string GeneratetrackingId()
        {
            try
            {
                Random random = new Random();
                long accountNumber = random.Next(1000, 9999);
                return accountNumber.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while generating tracking ID.", ex);
            }
        }
    }
}
