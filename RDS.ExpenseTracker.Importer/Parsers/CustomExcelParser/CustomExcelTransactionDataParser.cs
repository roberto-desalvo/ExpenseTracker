using RDS.ExpenseTracker.Domain.Models;
using System.Data;
using System.Diagnostics;
using System.Text;
using RDS.ExpenseTracker.Importer.Parsers.Abstractions;
using ExcelDataReader;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Models;
using RDS.ExpenseTracker.Importer.Utilities;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Helpers;

namespace RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser
{
    public class CustomExcelTransactionDataParser : ITransactionDataParser
    {
        private readonly CustomExcelImporterConfiguration _config;
        public CustomExcelImporterConfiguration Config => _config;

        #region Constructors
        public CustomExcelTransactionDataParser(CustomExcelImporterConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }
        #endregion

        #region Private Methods 
        private IEnumerable<Transaction> GetTransactionsFromRow(DataRow row)
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

        private IEnumerable<Transaction> GetTransactionsFromDataTable(DataTable dataTable)
        {
            var defaultDate = ParserHelper.ParseDateFromSheetName(dataTable.TableName);
            var dataRows = dataTable.Rows.Cast<DataRow>();
            var transactions = dataRows
                .SelectMany(GetTransactionsFromRow)
                .Select(transaction =>
            {
                AssignDateIfMissing(transaction, defaultDate);
                return transaction;
            });

            return transactions;
        }        

        private CustomExcelDataRowModel GetDataRowModel(DataRow dataRow)
        {
            var transactionOutflow = Utils.DecimalToCubedInt(dataRow[Config.TransactionOutflowIndex].ParseToDecimal()) ?? 0;
            var transactionInflow = Utils.DecimalToCubedInt(dataRow[Config.TransactionInflowIndex].ParseToDecimal()) ?? 0;

            var model = new CustomExcelDataRowModel
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

        private Transaction ExtractStandardTransaction(CustomExcelDataRowModel model)
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

        private Transaction ExtractOutgoingTransfer(CustomExcelDataRowModel model)
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

        private Transaction ExtractIngoingTransfer(CustomExcelDataRowModel rowModel)
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

        public static void AssignDateIfMissing(Transaction transaction, DateTime defaultDate)
        {
            var registeredDate = transaction.Date;
            transaction.Date = registeredDate == null ? defaultDate : new DateTime(defaultDate.Year, registeredDate.Value.Month, registeredDate.Value.Day);
        }
        #endregion

        #region Public Methods
        public IEnumerable<Transaction> ParseTransactions()
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using var stream = File.Open(_config.FilePath, FileMode.Open, FileAccess.Read);
                using var reader = ExcelReaderFactory.CreateReader(stream);

                var dataTables = reader.AsDataSet().Tables.Cast<DataTable>();
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
