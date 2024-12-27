
namespace RDS.ExpenseTracker.Business.TransactionImport.Abstractions
{
    public interface IXlsTransactionImporter : ITransactionImporter
    {
        Task ImportTransactions(bool deleteAll, bool updateAvailabilities);
    }
}