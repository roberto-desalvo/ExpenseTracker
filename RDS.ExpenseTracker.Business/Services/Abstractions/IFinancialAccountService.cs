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
        bool UpdateAvailability(string accountId, decimal amount);
        bool UpdateAvailability(int accountId, decimal amount);
        decimal GetAvailability(string accountId);
        decimal GetAvailability(int accountId);

    }
}
