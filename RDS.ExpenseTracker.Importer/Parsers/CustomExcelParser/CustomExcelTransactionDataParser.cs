using RDS.ExpenseTracker.Domain.Models;
using System.Data;
using System.Diagnostics;
using System.Text;
using RDS.ExpenseTracker.Importer.Parsers.Abstractions;
using ExcelDataReader;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Models;
using RDS.ExpenseTracker.Importer.Utilities;

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
            var model = CustomExcelParserUtilities.GetDataRowModel(row);

            if (model.TransactionAmount != 0 && !string.IsNullOrWhiteSpace(model.TransactionAccountName))
            {
                yield return CustomExcelParserUtilities.ExtractStandardTransaction(model);
            }

            if (model.TransferAmount != 0 && !string.IsNullOrWhiteSpace(model.TransferDescription))
            {
                yield return CustomExcelParserUtilities.ExtractOutgoingTransfer(model);
                yield return CustomExcelParserUtilities.ExtractIngoingTransfer(model);
            }
        }

        private IEnumerable<Transaction> GetTransactionsFromDataTable(DataTable dataTable)
        {
            var defaultDate = CustomExcelParserUtilities.ParseDateFromSheetName(dataTable.TableName);
            var dataRows = dataTable.Rows.Cast<DataRow>();
            var transactions = dataRows
                .SelectMany(GetTransactionsFromRow)
                .Select(transaction =>
            {
                CustomExcelParserUtilities.AssignDateIfMissing(transaction, defaultDate);
                return transaction;
            });

            return transactions;
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
                return Enumerable.Empty<Transaction>();
            }
        }


        #endregion
    }
}
