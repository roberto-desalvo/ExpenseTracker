using RDS.ExpenseTracker.Business.DataImport.Abstractions;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser;

namespace RDS.ExpenseTracker.Business.DataImport
{
    public class CustomExcelImportService : TransactionImportService
    {
        private readonly ITransactionImportService _importService;
        protected new ExcelTransactionDataParser _parser => (ExcelTransactionDataParser)base._parser;

        public CustomExcelImportService(ExcelTransactionDataParser parser, IFinancialAccountService accountService, ITransactionService transactionService, ICategoryService categoryService) : base(parser, accountService, transactionService, categoryService)
        {
            _importService = new TransactionImportService(parser, accountService, transactionService, categoryService);
        }

        public virtual async Task ImportTransactions()
        {
            await _transactionService.DeleteAllTransactions();
            await RestoreAccountBaseAmounts();

            var transactions = _importService.GetTransactions().ToList();

            await _importService.AssignAccounts(transactions).ConfigureAwait(false);
            await _importService.AssignCategories(transactions).ConfigureAwait(false);
            await _importService.SaveTransactions(transactions).ConfigureAwait(false);
            await _importService.UpdateAvailabilities(transactions).ConfigureAwait(false);
        }

        private async Task RestoreAccountBaseAmounts()
        {
            var accounts = await _accountService.GetFinancialAccounts();

            foreach (var key in _parser.Config.AccountInitialAmounts.Keys)
            {

                if (accounts.FirstOrDefault(a => string.Equals(a.Name, key, StringComparison.InvariantCultureIgnoreCase))
                    is not FinancialAccount account)
                {
                    throw new Exception($"Account {key} not found");
                }

                account.Availability = _parser.Config.AccountInitialAmounts[key];
                await _accountService.UpdateFinancialAccount(account);
            }
        }
    }
}

