using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDS.ExpenseTracker.Business.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public static class ExcelReaderUtilities
    {
        private static readonly string[] months = { "gennaio", "febbraio", "marzo", "aprile", "maggio", "giugno", "luglio", "agosto", "settembre", "ottobre", "novembre", "dicembre" };

        public static DateTime ParseDateFromSheetName(string name)
        {
            var index = name.IndexOf('2');
            var year = int.Parse(name[index..].Trim());
            var monthStr = name[..index].Trim().ToLower();

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
                FinancialAccountId = 0,
                Id = 0,
                IsTransfer = false,
                Category = CategoryHelper.GetCategory(model.TransactionDescription)
            };
        }

        public static Transaction GetOutgoingTransfer(this ExcelDataRowModel model)
        {
            return new Transaction
            {
                Amount = model.TransferAmount * -1,
                Date = model.TransferDate,
                Description = model.TransferDescription,
                Category = CategoryEnum.SpostamentiDenaro,
                FinancialAccountId = 0,
                FinancialAccountName = "Sella",
                Id = 0,
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
                Category = CategoryEnum.SpostamentiDenaro,
                FinancialAccountId = 0,
                FinancialAccountName = accountName,
                Id = 0,
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
