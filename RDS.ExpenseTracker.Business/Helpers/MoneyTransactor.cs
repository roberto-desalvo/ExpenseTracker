using Microsoft.EntityFrameworkCore;
using RDS.ExpenseTracker.Data;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public class MoneyTransactor : IMoneyTransactor
    {
        private readonly ExpenseTrackerContext _context;

        public MoneyTransactor(ExpenseTrackerContext context)
        {
            _context = context;
        }
        public bool AtomicTransaction(Action action)
        {
            using var dbTransaction = _context.Database.BeginTransaction();
            try
            {
                action.Invoke();
                dbTransaction.Commit();
                dbTransaction.Dispose();
                return true;
            }
            catch (Exception)
            {
                dbTransaction.Rollback();
                throw;
            }
        }
    }
}
