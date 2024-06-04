using ExcelDataReader;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public class ExcelReader : MoneyTransactor
    {
        private readonly IFinancialAccountService _accountService;
        private readonly IMoneyTransferService _transferService;
        private readonly ITransactionService _transactionService;

        #region Constructors
        public ExcelReader(IFinancialAccountService accountService, IMoneyTransferService transferService, ITransactionService transactionService, ExpenseTrackerContext context)
            : base(context)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _transferService = transferService ?? throw new ArgumentNullException(nameof(transferService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }
        #endregion

        #region Private Methods
        private MoneyTransfer? GetTransferFromRow(DataRow row)
        {
            var transferDate = row[9];
            var transferName = row[10];
            var transferAmount = row[11];

            var isTransferAmountFloat = decimal.TryParse(transferAmount.ToString(), out var transferAmountFloat);
            if (!isTransferAmountFloat)
            {
                return null;
            }
            transferAmountFloat = (decimal)Math.Round(transferAmountFloat, 2);

            var accounts = _accountService.GetFinancialAccounts();

            FinancialAccount destinationAccount = new();
            foreach (var acc in accounts)
            {
                if (transferName?.ToString().ToLower().Contains(acc.Name.ToLower()) ?? false)
                {
                    destinationAccount = acc;
                }
            }

            var dateIsSuccessfullyParsed = DateTime.TryParse(transferDate.ToString(), out var parsedDate);
            var sellaId = _accountService.GetFinancialAccounts(x => x.Name.ToLower() == "sella").FirstOrDefault()!.Id;

            var moneyTransfer = new MoneyTransfer()
            {
                Amount = transferAmountFloat,
                Date = dateIsSuccessfullyParsed ? parsedDate : null,
                DepositId = destinationAccount.Id,
                WithdrawId = sellaId,
                Description = transferName?.ToString() ?? string.Empty
            };
            return moneyTransfer;
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

            var accountName = row[4];
            var account = _accountService.GetFinancialAccounts(x => x.Name.ToLower() == accountName!.ToString().ToLower()).FirstOrDefault();

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
        public IList<Tuple<IList<MoneyTransfer>, IList<Transaction>>> GetTransactionsFromExcel(string path)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
                using var reader = ExcelReaderFactory.CreateReader(stream);
                var dataSet = reader.AsDataSet();

                var finalList = new List<Tuple<IList<MoneyTransfer>, IList<Transaction>>>();

                foreach (DataTable dataTable in dataSet.Tables)
                {
                    var tableName = dataTable.TableName;
                    if (tableName.Contains("Back-up") || tableName.Contains("Sheet"))
                    {
                        continue;
                    }
                    if ((tableName.ToLower() == "maggio 2021") || (tableName.ToLower() == "giugno 2021")
                        || (tableName.ToLower() == "luglio 2021") || (tableName.ToLower() == "agosto 2021"))
                    {
                        continue;
                    }

                    var date = Utilities.ParseDateFromSheetName(tableName);
                    var transactions = new List<Transaction>();
                    var transferList = new List<MoneyTransfer>();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var transaction = GetTransactionFromDataRow(row);

                        if (transaction != null)
                        {
                            if (transaction.Date == null)
                            {
                                transaction.Date = date;
                            };
                            transactions.Add(transaction);
                        }

                        var transfer = GetTransferFromRow(row);

                        if (transfer != null)
                        {
                            if (transfer.Date == null)
                            {
                                transfer.Date = date;
                            };
                            transferList.Add(transfer);
                        }
                    }

                    var tuple = new Tuple<IList<MoneyTransfer>, IList<Transaction>>(transferList, transactions);
                    finalList.Add(tuple);
                }

                return finalList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);
                return new List<Tuple<IList<MoneyTransfer>, IList<Transaction>>>();
            }
        }

        public void SaveData(IList<Tuple<IList<MoneyTransfer>, IList<Transaction>>> list)
        {
            AtomicTransaction(() =>
            {
                foreach (var tuple in list)
                {
                    foreach (var transfer in tuple.Item1)
                    {
                        _transferService.AddMoneyTransfer(transfer);
                    }
                    foreach (var transaction in tuple.Item2)
                    {
                        _transactionService.AddTransaction(transaction);
                    }
                }
            });
        }
        #endregion

    }
}
