using ExcelDataReader;
using RDS.ExpenseTracker.Business.TransactionImport.Parsers.Abstractions;
using RDS.ExpenseTracker.Business.TransactionImport.Parsers.Models;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Utilities;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace RDS.ExpenseTracker.Business.TransactionImport.Parsers
{
    public class XlsTransactionDataParser : ITransactionDataParser
    {
        private readonly XlsImporterConfiguration _config;

        #region Constructors
        public XlsTransactionDataParser(XlsImporterConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }
        #endregion

        #region Private Methods 
        private IEnumerable<Transaction> GetTransactionsFromRow(DataRow row)
        {
            var model = XlsParserUtilities.GetDataRowModel(row);

            if (model.TransactionAmount != 0 && !string.IsNullOrWhiteSpace(model.TransactionAccountName))
            {
                yield return XlsParserUtilities.ExtractStandardTransaction(model);
            }

            if (model.TransferAmount != 0 && !string.IsNullOrWhiteSpace(model.TransferDescription))
            {
                yield return XlsParserUtilities.ExtractOutgoingTransfer(model);
                yield return XlsParserUtilities.ExtractIngoingTransfer(model);
            }
        }

        private IEnumerable<Transaction> GetTransactionsFromDataTable(DataTable dataTable)
        {
            var defaultDate = XlsParserUtilities.ParseDateFromSheetName(dataTable.TableName);
            var dataRows = dataTable.Rows.Cast<DataRow>();
            var transactions = dataRows
                .SelectMany(GetTransactionsFromRow)
                .Select(transaction =>
            {
                XlsParserUtilities.AssignDateIfMissing(transaction, defaultDate);
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
