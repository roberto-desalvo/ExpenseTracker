using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Data
{
    public static class DataHelper 
    {        
        public static bool AtomicTransaction(Action action, ExpenseTrackerContext context)
        {
            using var dbTransaction = context.Database.BeginTransaction();
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
