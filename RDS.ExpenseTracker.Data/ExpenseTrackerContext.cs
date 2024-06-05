using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using RDS.ExpenseTracker.Data.Entities;

namespace RDS.ExpenseTracker.Data
{
    public class ExpenseTrackerContext : DbContext
    {
        public DbSet<ETransaction> Transactions { get; set; }
        public DbSet<EFinancialAccount> FinancialAccounts { get; set; }

        #region Constructors
        public ExpenseTrackerContext()
        {

        }
        public ExpenseTrackerContext(DbContextOptions<ExpenseTrackerContext> opt) : base(opt)
        {

        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EFinancialAccount>()
                .HasMany(x => x.Transactions)
                .WithOne(x => x.FinancialAccount)
                .HasForeignKey(x => x.FinancialAccountId)
            .OnDelete(DeleteBehavior.Restrict);                
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 2);
        }
    }
}
