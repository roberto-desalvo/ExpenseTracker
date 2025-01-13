using System.Data;
using System.Globalization;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Models;
using RDS.ExpenseTracker.Importer.Utilities;

namespace RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser
{
    public static class CustomExcelParserUtilities
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

        public static CustomExcelDataRowModel GetDataRowModel(DataRow dataRow)
        {
            var transactionOutflow = Utils.DecimalToCubedInt(dataRow[CustomExcelConstants.TransactionOutflowIndex].ParseToDecimal()) ?? 0;
            var transactionInflow = Utils.DecimalToCubedInt(dataRow[CustomExcelConstants.TransactionInflowIndex].ParseToDecimal()) ?? 0;

            var model = new CustomExcelDataRowModel
            {
                TransactionDate = dataRow[CustomExcelConstants.TransactionDateIndex].ParseToDateTime(),
                TransactionDescription = dataRow[CustomExcelConstants.TransactionDescriptionIndex]?.ToString() ?? string.Empty,
                TransactionAmount = transactionOutflow > 0 ? transactionOutflow * -1 : transactionInflow > 0 ? transactionInflow : 0,
                TransactionAccountName = dataRow[CustomExcelConstants.TransactionAccountNameIndex]?.ToString() ?? string.Empty,
                TransferDate = dataRow[CustomExcelConstants.TransferDateIndex].ParseToDateTime(),
                TransferDescription = dataRow[CustomExcelConstants.TransferDescriptionIndex]?.ToString() ?? string.Empty,
                TransferAmount = Utils.DecimalToCubedInt(dataRow[CustomExcelConstants.TransferAmountIndex].ParseToDecimal()) ?? 0
            };

            return model;
        }

        public static Transaction ExtractStandardTransaction(CustomExcelDataRowModel model)
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

        public static Transaction ExtractOutgoingTransfer(CustomExcelDataRowModel model)
        {
            var transaction = new Transaction
            {
                Amount = model.TransferAmount * -1,
                Date = model.TransferDate,
                Description = model.TransferDescription,
                FinancialAccountName = CustomExcelConstants.SellaAccountName,
                IsTransfer = true
            };

            return transaction;
        }

        public static Transaction ExtractIngoingTransfer(CustomExcelDataRowModel rowModel)
        {
            var accountName = rowModel.TransferDescription.ToLower().Contains(CustomExcelConstants.HypeAccountName.ToLower())
                ? CustomExcelConstants.HypeAccountName
                : rowModel.TransferDescription.ToLower().Contains(CustomExcelConstants.SatispayAccountName.ToLower())
                ? CustomExcelConstants.SatispayAccountName : string.Empty;

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
