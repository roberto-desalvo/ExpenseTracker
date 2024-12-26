using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Data.Entities;

namespace RDS.ExpenseTracker.Business.Services.Abstractions
{
    public interface ITransactionService
    {
        Task<Transaction?> GetTransaction(int id);
        Task<IEnumerable<Transaction>> GetTransactions(Func<IQueryable<ETransaction>, IQueryable<ETransaction>>? filter = null);
        Task AddTransactions(IEnumerable<Transaction> transactions);
        Task AddTransaction(Transaction transaction, bool saveChanges);
        Task UpdateTransaction(Transaction transaction);
        Task DeleteTransaction(int id);
        Task DeleteAllTransactions();
    }
}
