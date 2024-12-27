using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.TransactionImport.Abstractions
{
    public interface ITransactionImporter
    {
        Task ImportTransactions();
        Task ImportTransactions(bool updateAvailabilities);
    }
}
