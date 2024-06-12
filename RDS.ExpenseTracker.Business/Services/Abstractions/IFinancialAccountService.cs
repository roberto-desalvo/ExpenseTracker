using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Data.Entities;

namespace RDS.ExpenseTracker.Business.Services.Abstractions
{
    public interface IFinancialAccountService
    {
        int AddFinancialAccount(FinancialAccount account);
        void DeleteFinancialAccount(string id);
        void DeleteFinancialAccount(int id);
        void UpdateFinancialAccount(FinancialAccount account);
        FinancialAccount? GetFinancialAccount(string id);
        FinancialAccount? GetFinancialAccount(int id);
        IList<FinancialAccount> GetFinancialAccounts(Func<EFinancialAccount, bool>? filter = null);
        bool UpdateAvailability(string accountId, int amount, bool saveChanges);
        bool UpdateAvailability(int accountId, int amount, bool saveChanges);
        decimal GetAvailability(string accountId);
        decimal GetAvailability(int accountId);
        void UpdateAvailabilities(IEnumerable<Transaction> transactions);
    }
}
