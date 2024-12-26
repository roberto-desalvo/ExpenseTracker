using RDS.ExpenseTracker.Business.Models;

namespace RDS.ExpenseTracker.Business.Helpers.Abstractions
{
    public interface ICustomExcelReader
    {
        Task SaveData(IEnumerable<Transaction> transactions);
        IEnumerable<Transaction> GetTransactionsFromExcel(string path);
    }
}
