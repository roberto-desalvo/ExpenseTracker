using RDS.ExpenseTracker.Business.Models;

namespace RDS.ExpenseTracker.Business.Services.Abstractions
{
    public interface IMoneyTransferService
    {
        void AddMoneyTransfer(MoneyTransfer moneyTransfer);
        void DeleteMoneyTransfer(string id);
        MoneyTransfer? GetMoneyTransfer(string id);
        IList<MoneyTransfer> GetMoneyTransfers();
        void UpdateMoneyTransfer(MoneyTransfer moneyTransfer);
    }
}
