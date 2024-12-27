using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Business.TransactionImport.Abstractions;
using RDS.ExpenseTracker.Business.TransactionImport.Exceptions;
using RDS.ExpenseTracker.Business.TransactionImport.Parsers;
using RDS.ExpenseTracker.Business.TransactionImport.Parsers.Models;

namespace RDS.ExpenseTracker.Business.TransactionImport
{
    public class XlsTransactionImporter : TransactionImporter, IXlsTransactionImporter
    {
        private readonly XlsImporterConfiguration _config;
        public XlsTransactionImporter(XlsImporterConfiguration config, IFinancialAccountService accountService, ITransactionService transactionService, ICategoryService categoryService) 
            : base(new XlsTransactionDataParser(config), accountService, transactionService, categoryService)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        private async Task RestoreAccountBaseAmounts()
        {
            var accounts = await _accountService.GetFinancialAccounts();

            foreach (var key in _config.AccountInitialAmounts.Keys)
            {

                if (accounts.FirstOrDefault(a => string.Equals(a.Name, key, StringComparison.InvariantCultureIgnoreCase))
                    is not FinancialAccount account)
                {
                    throw new ImportTransactionException($"Account {key} not found");
                }

                account.Availability = _config.AccountInitialAmounts[key];
                await _accountService.UpdateFinancialAccount(account);
            }
        }

        public async Task ImportTransactions(bool deleteAll, bool updateAvailabilities)
        {
            if (deleteAll)
            {
                await _transactionService.DeleteAllTransactions();
                await RestoreAccountBaseAmounts();
            }

            await base.ImportTransactions(updateAvailabilities);
        }        
    }
}
