using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RDS.ExpenseTracker.Data.Entities;

namespace RDS.ExpenseTracker.Data
{
    public class ExpenseTrackerContext : DbContext
    {
        public DbSet<ETransaction> Transactions { get; set; }
        public DbSet<EFinancialAccount> FinancialAccounts { get; set; }
        public DbSet<EMoneyTransfer> MoneyTransfers { get; set; }

        #region Constructors
        public ExpenseTrackerContext()
        {

        }
        public ExpenseTrackerContext(DbContextOptions<ExpenseTrackerContext> opt) : base(opt)
        {

        }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=MAIN;Database=ExpenseTracker_Main2;User Id=fantadepo;Password=fantadepo;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EFinancialAccount>()
                .HasMany<ETransaction>(x => x.Transactions)
                .WithOne(x => x.FinancialAccount)
                .HasForeignKey(x => x.FinancialAccountId)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<EMoneyTransfer>()
                .HasOne<ETransaction>(x => x.Deposit)
                .WithOne(x => x.DepositMoneyTransfer)
                .HasForeignKey<EMoneyTransfer>(x => x.DepositId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EMoneyTransfer>()
                .HasOne<ETransaction>(x => x.Withdraw)
                .WithOne(x => x.WithdrawMoneyTransfer)
                .HasForeignKey<EMoneyTransfer>(x => x.WithdrawId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ETransaction>(e => e.ToTable(nameof(Transactions), 
                t => t.HasTrigger("TR_Update_Availability_AfterInsert")));

            modelBuilder.Entity<ETransaction>(e => e.ToTable(nameof(Transactions), 
                t => t.HasTrigger("TR_Update_Availability_AfterUpdate")));

            modelBuilder.Entity<ETransaction>(e => e.ToTable(nameof(Transactions), 
                t => t.HasTrigger("TR_Update_Availability_AfterDelete")));
                
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 2);
        }
    }
}
