using ExcelDataReader;
using RDS.ExpenseTracker.Business.Helpers.Abstractions;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public class CustomExcelReader : ICustomExcelReader
    {
        private readonly IFinancialAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly string[] sheetNameExceptions;

        #region Constructors
        public CustomExcelReader(IFinancialAccountService accountService, ITransactionService transactionService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            sheetNameExceptions = new[]{ "back-up", "sheet", "maggio 2021", "giugno 2021", "luglio 2021", "agosto 2021" };
        }
        #endregion

        #region Private Methods 
        private IEnumerable<Transaction> GetTransactionsFromRow(DataRow row)
        {
            var model = row.ToExcelDataRowModel();

            if (model.TransactionAmount != 0 && !string.IsNullOrWhiteSpace(model.TransactionAccountName))
            {
                yield return model.GetStandardTransaction();
            }

            if (model.TransferAmount != 0 && !string.IsNullOrWhiteSpace(model.TransferDescription))
            {
                yield return model.GetOutgoingTransfer();
                yield return model.GetIngoingTransfer();
            }
        }

        private IEnumerable<Transaction> GetTransactionsFromDataTable(DataTable dataTable)
        {
            var defaultDate = ExcelReaderUtilities.ParseDateFromSheetName(dataTable.TableName);

            return dataTable.Rows.Cast<DataRow>()
                .SelectMany(GetTransactionsFromRow)
                .Select(transaction =>
            {
                ExcelReaderUtilities.AssignDateIfMissing(transaction, defaultDate);
                return transaction;
            });
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
                    .Where(dt => !dt.TableName.ToLower().ContainsOne(sheetNameExceptions))
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
