using Microsoft.EntityFrameworkCore;
using RDS.ExpenseTracker.Data.Entities;
using RDS.ExpenseTracker.Data.Helpers;

namespace RDS.ExpenseTracker.Data
{
    public class ExpenseTrackerContext : DbContext
    {
        public DbSet<ETransaction> Transactions { get; set; }
        public DbSet<EFinancialAccount> FinancialAccounts { get; set; }
        public DbSet<ECategory> Categories { get; set; }

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

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EFinancialAccount>()
                .HasMany(x => x.Transactions)
                .WithOne(x => x.FinancialAccount)
                .HasForeignKey(x => x.FinancialAccountId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ECategory>()
                .HasMany(x => x.Transactions)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ECategory>()
                .HasData(SeedingHelper.GetSeedCategories());
        }
    }
}
