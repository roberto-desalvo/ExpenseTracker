using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.DataAccess.Entities;

namespace RDS.ExpenseTracker.Business.Services.Abstractions
{
    public interface IFinancialAccountService
    {
        Task<int> AddFinancialAccount(FinancialAccount account);
        Task DeleteFinancialAccount(int id);
        Task UpdateFinancialAccount(FinancialAccount account);
        Task<FinancialAccount?> GetFinancialAccount(int id);
        Task<IEnumerable<FinancialAccount>> GetFinancialAccounts();
        Task<IEnumerable<FinancialAccount>> GetFinancialAccounts(Func<IQueryable<EFinancialAccount>, IQueryable<EFinancialAccount>> filter);
        Task<bool> UpdateAvailability(int accountId, int amount, bool saveChanges);
        Task<decimal> GetAvailability(int accountId);
        Task CalculateAvailabilities(IEnumerable<Transaction> transactions);
    }
}
