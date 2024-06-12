using ExcelDataReader;
using RDS.ExpenseTracker.Business.Helpers.Abstractions;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using System;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public class CustomExcelReader : ICustomExcelReader
    {
        private readonly IFinancialAccountService _accountService;
        private readonly ITransactionService _transactionService;

        #region Constructors
        public CustomExcelReader(IFinancialAccountService accountService, ITransactionService transactionService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
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
                    IsTransfer = false,
                    Category = CategoryHelper.GetCategory(rowModel.TransactionDescription)
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

        private IEnumerable<Transaction> GetTransactionsFromDataTable(DataTable dataTable)
        {
            var transactions = dataTable.Rows.Cast<DataRow>().SelectMany(GetTransactionsFromRow);

            var defaultDate = ExcelReaderUtilities.ParseDateFromSheetName(dataTable.TableName);
            AssignDateIfMissing(transactions, defaultDate);
            
            return transactions;
        }

        private void AssignDateIfMissing(IEnumerable<Transaction> transactions, DateTime defaultDate)
        {
            foreach (var transaction in transactions)
            {
                var registeredDate = transaction.Date;
                transaction.Date = (registeredDate == null) ? defaultDate : new DateTime(defaultDate.Year, registeredDate.Value.Month, registeredDate.Value.Day);
            }
        }

        private void AssignCreateFinancialAccount(IEnumerable<Transaction> transactions)
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

                var dataTables = reader.AsDataSet().Tables.Cast<DataTable>();

                return dataTables
                    .Where(dt => !dt.TableName.ToLower().ContainsOne(new string[] { "back-up", "sheet", "maggio 2021", "giugno 2021", "luglio 2021", "agosto 2021" }))
                    .SelectMany(GetTransactionsFromDataTable);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);
                return Enumerable.Empty<Transaction>();
            }
        }

        public void SaveData(IEnumerable<Transaction> transactions)
        {
            AssignCreateFinancialAccount(transactions);
            _transactionService.AddTransactions(transactions);
            _accountService.UpdateAvailabilities(transactions);
        }        
        #endregion

    }
}
