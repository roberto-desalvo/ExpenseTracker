using RDS.ExpenseTracker.Business.Models;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public interface ICustomExcelReader
    {
        void SaveData(IEnumerable<Transaction> transactions);
        IEnumerable<Transaction> GetTransactionsFromExcel(string path);
    }
}
