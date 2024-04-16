using CaseStudyFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseStudyFinal.Data
{
    public class CaseStudyFinalContext: DbContext
    {
        public CaseStudyFinalContext(DbContextOptions<CaseStudyFinalContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<LoanDetails> Loans { get; set; }
        public DbSet<ApplicationTracking> ApplicationTracking { get; set; }
        public DbSet<PersonalDetails> PersonalDetails { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Documents> Documents { get; set; }

    }
}
