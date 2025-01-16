using RDS.ExpenseTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.DataImport.Abstractions
{
    public interface ITransactionImportService
    {
        IEnumerable<Transaction> GetTransactions();
        Task SaveTransactions(IList<Transaction> transactions);
        Task AssignAccounts(IList<Transaction> transactions);
        Task AssignAccount(IList<Transaction> transactions, bool createIfMissing);
        Task AssignCategories(IList<Transaction> transactions);
        Task UpdateAvailabilities(IList<Transaction> transactions);
    }
}
