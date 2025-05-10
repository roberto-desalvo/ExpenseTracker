using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.DataAccess.Entities;

namespace RDS.ExpenseTracker.Business.Services.Abstractions
{
    public interface ITransactionService
    {
        Task<Transaction?> GetTransaction(int id);
        Task<IEnumerable<Transaction>> GetTransactions();
        Task<IEnumerable<Transaction>> GetTransactions(Func<IQueryable<ETransaction>, IQueryable<ETransaction>> filter);
        Task AddTransactions(IEnumerable<Transaction> transactions);
        Task UpdateTransaction(Transaction transaction);
        Task DeleteTransaction(int id);
        Task DeleteAllTransactions();
        Task<int> AddTransaction(Transaction transaction);
        Task<int> AddTransaction(Transaction transaction, bool saveChanges);
        Task ResetTransactions(IEnumerable<Transaction> transactions);
    }
}
