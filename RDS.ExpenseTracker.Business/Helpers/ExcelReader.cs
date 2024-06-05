using ExcelDataReader;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public class ExcelReader 
    {
        private readonly IFinancialAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly ExpenseTrackerContext _context;

        #region Constructors
        public ExcelReader(IFinancialAccountService accountService, ITransactionService transactionService, ExpenseTrackerContext context)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region Private Methods
        private List<Transaction>? GetTransferTransactionsFromDataRow(DataRow row)
        {
            var transferDate = row[9];
            var transferName = row[10]?.ToString() ?? string.Empty;
            var transferAmount = row[11];

            var isTransferAmountDecimal = decimal.TryParse(transferAmount.ToString(), out var transferAmountDecimal);
            if (!isTransferAmountDecimal)
            {
                return null;
            }
            transferAmountDecimal = (decimal)Math.Round(transferAmountDecimal, 2);

            var accounts = _accountService.GetFinancialAccounts();

            var destinationAccount = new FinancialAccount();
            foreach (var acc in accounts)
            {
                if (transferName.ToLower().Contains(acc.Name.ToLower()))
                {
                    destinationAccount = acc;
                }
            }

            var dateIsSuccessfullyParsed = DateTime.TryParse(transferDate.ToString(), out var parsedDate);
            var sellaId = _accountService.GetFinancialAccounts(x => x.Name.ToLower() == "sella").FirstOrDefault()!.Id;

            var originTransaction = new Transaction
            {
                Amount = transferAmountDecimal * -1,
                Date = dateIsSuccessfullyParsed ? parsedDate : null,
                Description = transferName,
                Category = CategoryEnum.SpostamentiDenaro,
                FinancialAccountId = sellaId,
                FinancialAccountName = "Sella",
                Id = 0,
                IsTransfer = true
            };

            var destinationTransaction = new Transaction
            {
                Amount = transferAmountDecimal,
                Date = dateIsSuccessfullyParsed ? parsedDate : null,
                Description = transferName,
                Category = CategoryEnum.SpostamentiDenaro,
                FinancialAccountId = destinationAccount.Id,
                FinancialAccountName = destinationAccount.Name,
                Id = 0,
                IsTransfer = true
            };
            
            return new List<Transaction> { originTransaction, destinationTransaction };
        }

        private Transaction? GetTransactionFromDataRow(DataRow row)
        {
            var outflow = row[2];
            var ouflowIsNotFloat = !decimal.TryParse(outflow.ToString(), out var parsedOutflow);

            var inflow = row[3];
            var inflowIsNotFloat = !decimal.TryParse(inflow.ToString(), out var parsedInflow);
            if (ouflowIsNotFloat && inflowIsNotFloat)
            {
                return null;
            }

            parsedOutflow = (decimal)Math.Round(parsedOutflow, 2);
            parsedInflow = (decimal)Math.Round(parsedInflow, 2);

            var accountName = row[4]?.ToString() ?? string.Empty;
            var account = _accountService.GetFinancialAccounts(x => x.Name.ToLower() == accountName.ToLower()).FirstOrDefault();

            int accountId = 0;
            if (account is null)
            {
                var newAccount = new FinancialAccount()
                {
                    Name = accountName?.ToString() ?? string.Empty,
                    Availability = 0
                };
                accountId = _accountService.AddFinancialAccount(newAccount);
            }

            var date = row[0];
            var dateIsSuccessfullyParsed = DateTime.TryParse(date.ToString(), out var parsedDate);

            var reason = row[1];
            var transaction = new Transaction
            {
                Date = dateIsSuccessfullyParsed ? parsedDate : null,
                Description = reason.ToString(),
                Amount = parsedOutflow == 0 ? parsedInflow : -parsedOutflow,
                FinancialAccountId = account is null ? accountId : account.Id
            };

            return transaction;
        }

        
        #endregion

        #region Public Methods
        public IList<Transaction> GetTransactionsFromExcel(string path)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
                using var reader = ExcelReaderFactory.CreateReader(stream);
                var dataSet = reader.AsDataSet();

                var transactions = new List<Transaction>();

                foreach (DataTable dataTable in dataSet.Tables)
                {
                    var exceptions =  new string[] { "back-up", "sheet", "maggio 2021", "giugno 2021", "luglio 2021", "agosto 2021" };
                    if (dataTable.TableName.ToLower().ContainsOne(exceptions))
                    {
                        continue;
                    }

                    var currentSheetTransactions = new List<Transaction>();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var transaction = GetTransactionFromDataRow(row);

                        if (transaction != null)
                        {
                            currentSheetTransactions.Add(transaction);
                        }

                        var transferTransactions = GetTransferTransactionsFromDataRow(row);

                        if (transferTransactions != null)
                        {
                            currentSheetTransactions = currentSheetTransactions.Concat(transferTransactions).ToList();
                        }
                    }

                    var date = Utilities.ParseDateFromSheetName(dataTable.TableName);

                    foreach(var transaction in currentSheetTransactions)
                    {
                        transaction.Date ??= date;
                    }

                    transactions = transactions.Concat(currentSheetTransactions).ToList();
                }

                return transactions;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);
                return new List<Transaction>();
            }
        }

        public void SaveData(IList<Transaction> transactions)
        {
            DataHelper.AtomicTransaction(() =>
            {
                foreach (var transaction in transactions)
                {                    
                    _transactionService.AddTransaction(transaction);                    
                }
            }, _context);
        }
        #endregion

    }
}
