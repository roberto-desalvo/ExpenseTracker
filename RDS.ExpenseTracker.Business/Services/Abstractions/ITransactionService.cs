using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.DataAccess.Entities;

namespace RDS.ExpenseTracker.Business.Services.Abstractions
{
    public interface ITransactionService
    {
        Task<Domain.Models.Transaction?> GetTransaction(int id);
        Task<IEnumerable<Domain.Models.Transaction>> GetTransactions();
        Task<IEnumerable<Domain.Models.Transaction>> GetTransactions(Func<IQueryable<DataAccess.Entities.Transaction>, IQueryable<DataAccess.Entities.Transaction>> filter);
        Task AddTransactions(IEnumerable<Domain.Models.Transaction> transactions);
        Task UpdateTransaction(Domain.Models.Transaction transaction);
        Task DeleteTransaction(int id);
        Task DeleteAllTransactions();
        Task<int> AddTransaction(Domain.Models.Transaction transaction);
        Task<int> AddTransaction(Domain.Models.Transaction transaction, bool saveChanges);
        Task ResetTransactions(IEnumerable<Domain.Models.Transaction> transactions);
    }
}
