using System.Data;
using System.Globalization;
using RDS.ExpenseTracker.Business.TransactionImport.Parsers.Models;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Utilities;

namespace RDS.ExpenseTracker.Business.TransactionImport.Parsers
{
    public static class XlsParserUtilities
    {
        public static DateTime ParseDateFromSheetName(string name)
        {
            var index = name.IndexOf('2');
            var year = int.Parse(name[index..].Trim());
            var monthStr = name[..index].Trim().ToLower();

            var months = new CultureInfo("it-IT").DateTimeFormat.MonthNames.Select(x => x.ToLowerInvariant()).ToArray();
            var month = Array.IndexOf(months, monthStr) + 1;

            return new DateTime(year, month, 1);
        }

        public static XlsDataRowModel GetDataRowModel(DataRow dataRow)
        {
            var transactionOutflow = Utils.DecimalToCubedInt(dataRow[XlsConstants.TransactionOutflowIndex].ParseToDecimal()) ?? 0;
            var transactionInflow = Utils.DecimalToCubedInt(dataRow[XlsConstants.TransactionInflowIndex].ParseToDecimal()) ?? 0;

            var model = new XlsDataRowModel
            {
                TransactionDate = dataRow[XlsConstants.TransactionDateIndex].ParseToDateTime(),
                TransactionDescription = dataRow[XlsConstants.TransactionDescriptionIndex]?.ToString() ?? string.Empty,
                TransactionAmount = transactionOutflow > 0 ? transactionOutflow * -1 : transactionInflow > 0 ? transactionInflow : 0,
                TransactionAccountName = dataRow[XlsConstants.TransactionAccountNameIndex]?.ToString() ?? string.Empty,
                TransferDate = dataRow[XlsConstants.TransferDateIndex].ParseToDateTime(),
                TransferDescription = dataRow[XlsConstants.TransferDescriptionIndex]?.ToString() ?? string.Empty,
                TransferAmount = Utils.DecimalToCubedInt(dataRow[XlsConstants.TransferAmountIndex].ParseToDecimal()) ?? 0
            };

            return model;
        }

        public static Transaction ExtractStandardTransaction(XlsDataRowModel model)
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

        public static Transaction ExtractOutgoingTransfer(XlsDataRowModel model)
        {
            var transaction = new Transaction
            {
                Amount = model.TransferAmount * -1,
                Date = model.TransferDate,
                Description = model.TransferDescription,
                FinancialAccountName = XlsConstants.SellaAccountName,
                IsTransfer = true
            };

            return transaction;
        }

        public static Transaction ExtractIngoingTransfer(XlsDataRowModel rowModel)
        {
            var accountName = rowModel.TransferDescription.ToLower().Contains(XlsConstants.HypeAccountName.ToLower())
                ? XlsConstants.HypeAccountName
                : rowModel.TransferDescription.ToLower().Contains(XlsConstants.SatispayAccountName.ToLower())
                ? XlsConstants.SatispayAccountName : string.Empty;

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
    }
}
