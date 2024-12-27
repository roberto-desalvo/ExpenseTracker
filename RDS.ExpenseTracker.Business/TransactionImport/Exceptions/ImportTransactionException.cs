using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.TransactionImport.Exceptions
{
    public class ImportTransactionException : Exception
    {
        public ImportTransactionException(string? message) : base(message)
        {
        }
    }
}
