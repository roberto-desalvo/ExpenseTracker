using RDS.ExpenseTracker.Business.TransactionImport.Abstractions;
using RDS.ExpenseTracker.Business.TransactionImport.Parsers.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;

namespace RDS.ExpenseTracker.Business.TransactionImport
{
    public class TransactionImporterFactory : ITransactionImporterFactory
    {
        private readonly IFinancialAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;

        public TransactionImporterFactory(IFinancialAccountService accountService, ITransactionService transactionService, ICategoryService categoryService) 
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        public IXlsTransactionImporter CreateXlsImporter(XlsImporterConfiguration configuration)
        {
            return new XlsTransactionImporter(configuration, _accountService, _transactionService, _categoryService);
        }
    }
}
