using System;
using System.Linq;
using System.Threading.Tasks;
using CaseStudyFinal.Data;
using CaseStudyFinal.DTO;
using CaseStudyFinal.Interface;
using CaseStudyFinal.Models;
using CaseStudyFinal.Service;
using Microsoft.EntityFrameworkCore;

namespace CaseStudyFinal.Repositories
{
    public class ApplicationTrackingRepository : IApplicationTrackingRepository
    {
        private readonly CaseStudyFinalContext _context;
        private readonly EmailService _emailService;

        public ApplicationTrackingRepository(CaseStudyFinalContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IEnumerable<ApplicationTracking>> GetAllApplications()
        {
            return await _context.ApplicationTracking.ToListAsync();
        }

        public async Task<ApplicationTracking> GetApplicationById(string trackerId)
        {
            var appl = await _context.ApplicationTracking.FirstOrDefaultAsync(at => at.TrackerID == trackerId);
            if (appl == null)
            {
                return null;
            }
            return appl;
                                
        }

        public async Task<string> Update(string trackerId, UpdateStatusDto details, string Email)
        {
            var update = await _context.ApplicationTracking.FirstOrDefaultAsync(x => x.TrackerID == trackerId);

            if (update != null)
            {
                update.ApplicationID = details.ApplicationID;
                update.AadharNo = details.AadharNo;
                update.Status = details.Status;
                
                await _context.SaveChangesAsync();
                if (update.Status == "Approved")
                {
                    var loanDetails = await _context.Loans.FirstOrDefaultAsync(ld => ld.ApplicationId == update.ApplicationID);
                    string aadharNo = update.AadharNo;
                    if (loanDetails == null)
                    {
                        throw new InvalidOperationException("Loan Details not found");
                    }

                    var account = new Account
                    {
                        AccountNumber = GenerateAccountNumber(),
                        TrackerID = update.TrackerID,
                        Balance = loanDetails.LoanAmount
                    };

                    _context.Accounts.Add(account);
                    await _context.SaveChangesAsync();

                    //var personalDetails = _context.PersonalDetails.FirstOrDefaultAsync(pd => pd.AadharNo == update.AadharNo);

                    string subject = "Home Loan Application was approved";
                    string body = $"Your Application has been Approved!. The approved Loan Amount has been credited to the account given : " +
                    $"\n ApplicationID: {update.ApplicationID}\nTrackingID: {update.TrackerID}\nAccount Number: {account.AccountNumber}.\n\n Regards,\nTeam HL Bank";

                    await _emailService.SendApplicationStatusEmail(update.TrackerID, Email, subject, body);
                }
                else if (update.Status == "Rejected")
                {
                    
                    string subject = "Home Loan Application was rejected";
                    string body = "We regret to inform you that your application was rejected";
                    await _emailService.SendApplicationStatusEmail(update.TrackerID, Email, subject, body);
                }
                else
                {
                    throw new InvalidOperationException("Error");
                }
                return "updated";
            }
            else
            {
                return "No Details Found";
            }
        
        }

       
        private string GenerateAccountNumber()
        {
            Random random = new Random();
            long accountNumber = random.Next(1000, 9999);
            return accountNumber.ToString();
        }
    }
    
}
