using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Data.Entities;

namespace RDS.ExpenseTracker.Business.Services.Abstractions
{
    public interface IFinancialAccountService
    {
        Task<int> AddFinancialAccount(FinancialAccount account);
        Task DeleteFinancialAccount(int id);
        Task UpdateFinancialAccount(FinancialAccount account);
        Task<FinancialAccount?> GetFinancialAccount(int id);
        Task<IEnumerable<FinancialAccount>> GetFinancialAccounts(Func<EFinancialAccount, bool>? filter = null);
        Task<bool> UpdateAvailability(int accountId, int amount, bool saveChanges);
        Task<decimal> GetAvailability(int accountId);
        Task CalculateAvailabilities(IEnumerable<Transaction> transactions);
    }
}
