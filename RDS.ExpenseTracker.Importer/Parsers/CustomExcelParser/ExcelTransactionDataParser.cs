using RDS.ExpenseTracker.Domain.Models;
using System.Data;
using System.Diagnostics;
using System.Text;
using RDS.ExpenseTracker.Importer.Parsers.Abstractions;
using ExcelDataReader;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Models;
using RDS.ExpenseTracker.Importer.Utilities;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Helpers;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RDS.ExpenseTracker.Importer.Tests")]

namespace RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser
{
    public class ExcelTransactionDataParser : ITransactionDataParser
    {
        private readonly ExcelImporterConfiguration _config;
        private readonly IExcelFileReader _excelFileReader;
        public ExcelImporterConfiguration Config => _config;

        #region Constructors
        protected ExcelTransactionDataParser() // for testing purposes only
        {
            
        }
        public ExcelTransactionDataParser(IExcelFileReader excelFileReader, ExcelImporterConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _excelFileReader = excelFileReader ?? throw new ArgumentNullException(nameof(excelFileReader));
        }
        #endregion

        #region Private Methods 
        internal IEnumerable<Transaction> GetTransactionsFromRow(DataRow row)
        {
            var model = GetDataRowModel(row);

            if (model.TransactionAmount != 0 && !string.IsNullOrWhiteSpace(model.TransactionAccountName))
            {
                yield return ExtractStandardTransaction(model);
            }

            if (model.TransferAmount != 0 && !string.IsNullOrWhiteSpace(model.TransferDescription))
            {
                yield return ExtractOutgoingTransfer(model);
                yield return ExtractIngoingTransfer(model);
            }
        }

        internal IEnumerable<Transaction> GetTransactionsFromDataTable(DataTable dataTable)
        {
            var defaultDate = ParserHelper.ParseDateFromSheetName(dataTable.TableName);
            var dataRows = dataTable.Rows.Cast<DataRow>();
            var transactions = dataRows
                .SelectMany(GetTransactionsFromRow)
                .Select(transaction =>
            {
                ParserHelper.CheckAndAssignDate(transaction, defaultDate);
                return transaction;
            });

            return transactions;
        }

        internal ExcelDataRowModel GetDataRowModel(DataRow dataRow)
        {
            var transactionOutflow = Utils.DecimalToCubedInt(dataRow[Config.TransactionOutflowIndex].ParseToDecimal()) ?? 0;
            var transactionInflow = Utils.DecimalToCubedInt(dataRow[Config.TransactionInflowIndex].ParseToDecimal()) ?? 0;

            var model = new ExcelDataRowModel
            {
                TransactionDate = dataRow[Config.TransactionDateIndex].ParseToDateTime(),
                TransactionDescription = dataRow[Config.TransactionDescriptionIndex]?.ToString() ?? string.Empty,
                TransactionAmount = transactionOutflow > 0 ? transactionOutflow * -1 : transactionInflow > 0 ? transactionInflow : 0,
                TransactionAccountName = dataRow[Config.TransactionAccountNameIndex]?.ToString() ?? string.Empty,
                TransferDate = dataRow[Config.TransferDateIndex].ParseToDateTime(),
                TransferDescription = dataRow[Config.TransferDescriptionIndex]?.ToString() ?? string.Empty,
                TransferAmount = Utils.DecimalToCubedInt(dataRow[Config.TransferAmountIndex].ParseToDecimal()) ?? 0
            };

            return model;
        }

        internal Transaction ExtractStandardTransaction(ExcelDataRowModel model)
        {
            var transaction = new Transaction
            {
                Amount = model.TransactionAmount,
                Date = model.TransactionDate,
                Description = model.TransactionDescription,
                FinancialAccountName = model.TransactionAccountName,
                IsTransfer = false,
                CategoryDescription = string.Empty
            };

            return transaction;
        }

        internal Transaction ExtractOutgoingTransfer(ExcelDataRowModel model)
        {
            var transaction = new Transaction
            {
                Amount = model.TransferAmount * -1,
                Date = model.TransferDate,
                Description = model.TransferDescription,
                FinancialAccountName = Config.FirstAccountName,
                IsTransfer = true
            };

            return transaction;
        }

        internal Transaction ExtractIngoingTransfer(ExcelDataRowModel rowModel)
        {
            var accountName = rowModel.TransferDescription.ToLower().Contains(Config.SecondAccountName.ToLower())
                ? Config.SecondAccountName
                : rowModel.TransferDescription.ToLower().Contains(Config.ThirdAccountName.ToLower())
                ? Config.ThirdAccountName : string.Empty;

            var transaction = new Transaction
            {
                Amount = rowModel.TransferAmount,
                Date = rowModel.TransferDate,
                Description = rowModel.TransferDescription,
                FinancialAccountName = accountName,
                IsTransfer = true
            };

            return transaction;
        }        
        #endregion

        #region Public Methods
        public IEnumerable<Transaction> ParseTransactions()
        {
            try
            {
                var dataSet = _excelFileReader.ReadExcelFile(_config.FilePath);
                var dataTables = dataSet.Tables.Cast<DataTable>();
                var sheetsToIgnore = _config.SheetsToIgnore.ToArray();

                var filteredTables = dataTables.Where(dt => !dt.TableName.ToLower().ContainsOne(sheetsToIgnore));

                var transactions = filteredTables.SelectMany(GetTransactionsFromDataTable);

                return transactions;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);
                return [];
            }
        }


        #endregion
    }
}
