using System.Data;
using System.Globalization;
using RDS.ExpenseTracker.Business.Models;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public static class ExcelReaderUtilities
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

        public static ExcelDataRowModel ToExcelDataRowModel(this DataRow dataRow)
        {
            var transactionOutflow = Utilities.CubedDecimalToInt(dataRow[2].ParseToDecimal()) ?? 0;
            var transactionInflow = Utilities.CubedDecimalToInt(dataRow[3].ParseToDecimal()) ?? 0;

            var model = new ExcelDataRowModel
            {
                TransactionDate = dataRow[0].ParseToDateTime(),
                TransactionDescription = dataRow[1]?.ToString() ?? string.Empty,
                TransactionAmount = transactionOutflow > 0 ? transactionOutflow * -1 : transactionInflow > 0 ? transactionInflow : 0,
                TransactionAccountName = dataRow[4]?.ToString() ?? string.Empty,
                TransferDate = dataRow[9].ParseToDateTime(),
                TransferDescription = dataRow[10]?.ToString() ?? string.Empty,
                TransferAmount = Utilities.CubedDecimalToInt(dataRow[11].ParseToDecimal()) ?? 0
            };

            return model;
        }

        public static Transaction GetStandardTransaction(this ExcelDataRowModel model)
        {
            return new Transaction
            {
                Amount = model.TransactionAmount,
                Date = model.TransactionDate,
                Description = model.TransactionDescription,
                FinancialAccountName = model.TransactionAccountName,
                IsTransfer = false,
                CategoryName = ""
            };
        }

        public static Transaction GetOutgoingTransfer(this ExcelDataRowModel model)
        {
            return new Transaction
            {
                Amount = model.TransferAmount * -1,
                Date = model.TransferDate,
                Description = model.TransferDescription,
                CategoryName = "SpostamentiDenaro",
                FinancialAccountName = "Sella",
                IsTransfer = true
            };
        }

        public static Transaction GetIngoingTransfer(this ExcelDataRowModel rowModel)
        {
            var accountName = rowModel.TransferDescription.ToLower().Contains("hype") ? "Hype" : rowModel.TransferDescription.ToLower().Contains("satispay") ? "Satispay" : string.Empty;
            return new Transaction
            {
                Amount = rowModel.TransferAmount,
                Date = rowModel.TransferDate,
                Description = rowModel.TransferDescription,
                CategoryName = "SpostamentiDenaro",
                FinancialAccountName = accountName,
                IsTransfer = true
            };
        }

        public static void AssignDateIfMissing(Transaction transaction, DateTime defaultDate)
        {
            var registeredDate = transaction.Date;
            transaction.Date = (registeredDate == null) ? defaultDate : new DateTime(defaultDate.Year, registeredDate.Value.Month, registeredDate.Value.Day);
        }
    }
}
