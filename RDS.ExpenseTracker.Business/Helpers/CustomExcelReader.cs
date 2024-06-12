using ExcelDataReader;
using RDS.ExpenseTracker.Business.Helpers.Abstractions;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public class CustomExcelReader : ICustomExcelReader
    {
        private readonly IFinancialAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly ExpenseTrackerContext _context;

        #region Constructors
        public CustomExcelReader(IFinancialAccountService accountService, ITransactionService transactionService, ExpenseTrackerContext context)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region Private Methods        

        private IEnumerable<Transaction> GetTransactionsFromRow(DataRow row)
        {
            var transactions = new List<Transaction>();
            var rowModel = row.ToExcelDataRowModel();

            if (rowModel.TransactionAmount != 0 && !string.IsNullOrWhiteSpace(rowModel.TransactionAccountName))
            {
                var account = _accountService.GetFinancialAccounts(x => x.Name.ToLower() == rowModel.TransactionAccountName.ToLower()).FirstOrDefault();

                var transaction = new Transaction
                {
                    Amount = rowModel.TransactionAmount,
                    Date = rowModel.TransactionDate,
                    Description = rowModel.TransactionDescription,
                    FinancialAccountName = rowModel.TransactionAccountName,
                    FinancialAccountId = account?.Id ?? 0,
                    Id = 0,
                    IsTransfer = false
                };

                transactions.Add(transaction);
            }

            if (rowModel.TransferAmount != 0 && !string.IsNullOrWhiteSpace(rowModel.TransferDescription))
            {
                var accounts = _accountService.GetFinancialAccounts();

                var destinationAccount = new FinancialAccount();
                foreach (var account in accounts)
                {
                    if (rowModel.TransferDescription.ToLower().Contains(account.Name.ToLower()))
                    {
                        destinationAccount = account;
                    }
                }

                if (destinationAccount.Id == 0)
                {
                    if (rowModel.TransferDescription.ToLower().Contains("hype"))
                    {
                        destinationAccount.Name = "Hype";
                    }
                    else if (rowModel.TransferDescription.ToLower().Contains("satispay"))
                    {
                        destinationAccount.Name = "Satispay";
                    }
                }

                var sellaAccount = accounts.Where(x => x.Name.ToLower() == "sella").FirstOrDefault();

                var originTransaction = new Transaction
                {
                    Amount = rowModel.TransferAmount * -1,
                    Date = rowModel.TransferDate,
                    Description = rowModel.TransferDescription,
                    Category = CategoryEnum.SpostamentiDenaro,
                    FinancialAccountId = sellaAccount?.Id ?? 0,
                    FinancialAccountName = "Sella",
                    Id = 0,
                    IsTransfer = true
                };

                var destinationTransaction = new Transaction
                {
                    Amount = rowModel.TransferAmount,
                    Date = rowModel.TransferDate,
                    Description = rowModel.TransferDescription,
                    Category = CategoryEnum.SpostamentiDenaro,
                    FinancialAccountId = destinationAccount.Id,
                    FinancialAccountName = destinationAccount.Name,
                    Id = 0,
                    IsTransfer = true
                };

                transactions.Add(originTransaction);
                transactions.Add(destinationTransaction);
            }

            return transactions;
        }
        #endregion

        #region Public Methods
        public IEnumerable<Transaction> GetTransactionsFromExcel(string path)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
                using var reader = ExcelReaderFactory.CreateReader(stream);
                var dataSet = reader.AsDataSet();

                var transactions = Enumerable.Empty<Transaction>();

                foreach (DataTable dataTable in dataSet.Tables)
                {
                    var exceptions = new string[] { "back-up", "sheet", "maggio 2021", "giugno 2021", "luglio 2021", "agosto 2021" };
                    if (dataTable.TableName.ToLower().ContainsOne(exceptions))
                    {
                        continue;
                    }

                    var currentSheetTransactions = Enumerable.Empty<Transaction>();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var currentRowTransactions = GetTransactionsFromRow(row);
                        currentSheetTransactions = currentSheetTransactions.Concat(currentRowTransactions).ToList();
                    }

                    var date = ExcelReaderUtilities.ParseDateFromSheetName(dataTable.TableName);

                    foreach (var transaction in currentSheetTransactions)
                    {
                        var registeredDate = transaction.Date;
                        transaction.Date = (registeredDate == null) ? date : new DateTime(date.Year, registeredDate.Value.Month, registeredDate.Value.Day);
                    }

                    //SaveData(currentSheetTransactions);

                    transactions = transactions.Concat(currentSheetTransactions).ToList();
                }

                CategoryHelper.UpdateCategories(transactions);
                return transactions;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);
                return Enumerable.Empty<Transaction>();
            }
        }

        public void SaveData(IEnumerable<Transaction> transactions)
        {
            
                foreach (var transaction in transactions)
                {
                    if (transaction.FinancialAccountId <= 0)
                    {
                        var account = _accountService.GetFinancialAccounts(x => x.Name.ToLowerInvariant() == transaction.FinancialAccountName.ToLowerInvariant()).FirstOrDefault();
                        if (account != null)
                        {
                            transaction.FinancialAccountId = account.Id;
                            continue;
                        }

                        var newAccountId = _accountService.AddFinancialAccount(new FinancialAccount { Name = transaction.FinancialAccountName });
                        transaction.FinancialAccountId = newAccountId;
                    }
                }
                _transactionService.AddTransactions(transactions);
                _accountService.UpdateAvailabilities(transactions);
        }
        #endregion

    }
}
