using RDS.ExpenseTracker.Business.DataImport.Abstractions;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.Importer.Parsers.Abstractions;
using RDS.ExpenseTracker.Importer.Utilities;

namespace RDS.ExpenseTracker.Business.DataImport
{
    public class TransactionImportService : ITransactionImportService
    {
        protected readonly ITransactionDataParser _parser;
        protected readonly IFinancialAccountService _accountService;
        protected readonly ITransactionService _transactionService;
        protected readonly ICategoryService _categoryService;

        public TransactionImportService(ITransactionDataParser parser, IFinancialAccountService accountService, ITransactionService transactionService, ICategoryService categoryService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public async Task AssignAccounts(IList<Transaction> transactions)
        {
            await AssignAccount(transactions, false);
        }

        public async Task AssignAccount(IList<Transaction> transactions, bool createIfMissing)
        {
            foreach (var transaction in transactions)
            {
                if (transaction.FinancialAccountId <= 0)
                {
                    var list = await _accountService.GetFinancialAccounts(
                            accounts => accounts.Where(x => x.Name.ToLower().Trim() == transaction.FinancialAccountName.ToLower().Trim()))
                        .ConfigureAwait(false);

                    var account = list.FirstOrDefault();

                    if (account != null)
                    {
                        transaction.FinancialAccountId = account.Id;
                        continue;
                    }

                    if (createIfMissing)
                    {
                        var newAccount = new FinancialAccount { Name = transaction.FinancialAccountName };
                        var newAccountId = Task.Run(async () => await _accountService.AddFinancialAccount(newAccount).ConfigureAwait(false)).Result;

                        transaction.FinancialAccountId = newAccountId;
                    }
                }
            }
        }

        public async Task AssignCategories(IList<Transaction> transactions)
        {
            var categories = await _categoryService.GetCategories().ConfigureAwait(false);
            var defaultCategory = await _categoryService.GetDefaultCategory().ConfigureAwait(false);

            foreach (var transaction in transactions)
            {
                var orderedCategories = categories.OrderBy(x => x.Priority, Comparer<int>.Default);

                foreach (var category in orderedCategories)
                {
                    var tags = category.Tags.Select(tag => tag.Trim()).Where(tag => !string.IsNullOrWhiteSpace(tag)).ToArray();

                    if (transaction.Description.ContainsOne(ignoreCase: true, tags))
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

        public IEnumerable<Transaction> GetTransactions()
        {
            return _parser.ParseTransactions();
        }

        public async Task UpdateAvailabilities(IList<Transaction> transactions)
        {
            await _accountService.CalculateAvailabilities(transactions).ConfigureAwait(false);
        }

        public async Task SaveTransactions(IList<Transaction> transactions)
        {
            await _transactionService.AddTransactions(transactions).ConfigureAwait(false);
        }
    }
}
