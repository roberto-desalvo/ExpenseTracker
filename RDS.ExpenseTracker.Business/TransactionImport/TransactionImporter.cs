using RDS.ExpenseTracker.Business.TransactionImport.Abstractions;
using RDS.ExpenseTracker.Business.TransactionImport.Parsers.Abstractions;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Business.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.TransactionImport
{
    public class TransactionImporter : ITransactionImporter
    {
        protected readonly ITransactionDataParser _parser;
        protected readonly IFinancialAccountService _accountService;
        protected readonly ITransactionService _transactionService;
        protected readonly ICategoryService _categoryService;

        public TransactionImporter(ITransactionDataParser parser, IFinancialAccountService accountService, ITransactionService transactionService, ICategoryService categoryService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        private void AssignCreateFinancialAccount(IList<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                if (transaction.FinancialAccountId <= 0)
                {
                    var account = Task.Run(async () =>
                        (await _accountService.GetFinancialAccounts(
                            accounts => accounts.Where(x => x.Name.ToLower().Trim() == transaction.FinancialAccountName.ToLower().Trim())))
                        .FirstOrDefault()
                    ).Result;

                    if (account != null)
                    {
                        transaction.FinancialAccountId = account.Id;
                        continue;
                    }

                    var newAccount = new FinancialAccount { Name = transaction.FinancialAccountName };
                    var newAccountId = Task.Run(async () => await _accountService.AddFinancialAccount(newAccount)).Result;

                    transaction.FinancialAccountId = newAccountId;
                }
            }
        }

        private void AssignCategories(IList<Transaction> transactions)
        {
            var categories = Task.Run(_categoryService.GetCategories).Result;
            var defaultCategory = Task.Run(_categoryService.GetDefaultCategory).Result;

            foreach (var transaction in transactions)
            {
                foreach (var category in categories.OrderBy(x => x.Priority, Comparer<int>.Default))
                {
                    if (transaction.Description.ContainsOne(category.Tags.Select(x => x.Trim()).ToArray()))
                    {
                        transaction.CategoryId = category.Id;
                        break;
                    }
                }

                if (transaction.CategoryId == 0 && defaultCategory != null)
                {
                    transaction.CategoryId = defaultCategory.Id;
                }
            }
        }

        public virtual async Task ImportTransactions()
        {
            await ImportTransactions(true);
        }

        public virtual async Task ImportTransactions(bool updateAvailabilities)
        {
            var transactions = _parser.ParseTransactions().ToList();
            
            AssignCreateFinancialAccount(transactions);
            AssignCategories(transactions);

            await _transactionService.AddTransactions(transactions);

            if (updateAvailabilities)
            {
                await _accountService.CalculateAvailabilities(transactions);
            }
        }
    }
}
