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
        public static Dictionary<string, int> Months = new Dictionary<string, int>
            {
                { "gennaio", 1 },
                { "febbraio", 2 },
                { "marzo", 3 },
                { "aprile", 4 },
                { "maggio", 5 },
                { "giugno", 6 },
                { "luglio", 7 },
                { "agosto", 8 },
                { "settembre", 9 },
                { "ottobre", 10 },
                { "novembre", 11 },
                { "dicembre", 12 }
            };

        public static DateTime ParseDateFromSheetName(string name)
        {
            var index = name.IndexOf('2');
            var year = int.Parse(name[index..].Trim());
            var monthStr = name[..index].Trim().ToLower();

            var month = Months[monthStr];

            return new DateTime(year, month, 1);
        }

        public static ExcelDataRowModel ToExcelDataRowModel(this DataRow dataRow)
        {
            var model = new ExcelDataRowModel();

            var transactionDate = dataRow[0];
            var transactionDateIsSuccessfullyParsed = DateTime.TryParse(transactionDate.ToString(), out var parsedTransactionDate);
            model.TransactionDate = transactionDateIsSuccessfullyParsed ? parsedTransactionDate : null;

            model.TransactionDescription = dataRow[1]?.ToString() ?? string.Empty;

            var transactionOutflow = dataRow[2];
            var transactionInflow = dataRow[3];

            var successfullyParsedOuflow = !decimal.TryParse(transactionOutflow.ToString(), out var parsedOutflow);
            var successfullyParsedInflow = !decimal.TryParse(transactionInflow.ToString(), out var parsedInflow);

            decimal transactionAmount = 0;
            if (successfullyParsedOuflow && parsedOutflow > 0)
            {
                transactionAmount = Math.Round(parsedOutflow, 2) * -1;
            }
            else if (successfullyParsedInflow && parsedInflow > 0)
            {
                transactionAmount = Math.Round(parsedInflow, 2);
            }

            model.TransactionAmount = transactionAmount;

            model.TransactionAccountName = dataRow[4]?.ToString() ?? string.Empty;

            var transferDate = dataRow[9];
            var transferDateIsSuccessfullyParsed = DateTime.TryParse(transferDate.ToString(), out var parsedTransferDate);
            model.TransferDate = transferDateIsSuccessfullyParsed ? parsedTransferDate : null;

            model.TransferDescription = dataRow[10]?.ToString() ?? string.Empty;

            var transferAmount = dataRow[11];
            var successfullyParsedTransferAmount = decimal.TryParse(transferAmount.ToString(), out var parsedTransferAmount);

            model.TransactionAmount = successfullyParsedTransferAmount ? Math.Round(parsedTransferAmount, 2) : 0;
            return model;
        }
    }
}
