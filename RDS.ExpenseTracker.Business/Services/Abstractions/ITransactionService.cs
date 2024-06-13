using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Data.Entities;

namespace RDS.ExpenseTracker.Business.Services.Abstractions
{
    public interface ITransactionService
    {
        Transaction? GetTransaction(int id);
        Transaction? GetTransaction(string id);
        IEnumerable<Transaction> GetTransactions(Func<ETransaction, bool>? filter = null, bool lazy = true);
        void AddTransactions(IEnumerable<Transaction> transactions);
        void AddTransaction(Transaction transaction, bool saveChanges);
        void UpdateTransaction(Transaction transaction);
        void DeleteTransaction(int id);
        void DeleteTransaction(string id);
        void DeleteTransaction(Transaction transaction);
    }
}
