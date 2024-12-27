using RDS.ExpenseTracker.Business.TransactionImport.Parsers.Models;

namespace RDS.ExpenseTracker.Business.TransactionImport.Abstractions
{
    public interface ITransactionImporterFactory
    {
        IXlsTransactionImporter CreateXlsImporter(XlsImporterConfiguration configuration);
    }
}