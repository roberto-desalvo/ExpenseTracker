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
            var transactionOutflow = DecimalToInt(dataRow[2].ParseToDecimal()) ?? 0;
            var transactionInflow = DecimalToInt(dataRow[3].ParseToDecimal()) ?? 0;

            var model = new ExcelDataRowModel
            {
                TransactionDate = dataRow[0].ParseToDateTime(),
                TransactionDescription = dataRow[1]?.ToString() ?? string.Empty,
                TransactionAmount = transactionOutflow > 0 ? transactionOutflow * -1 : transactionInflow > 0 ? transactionInflow : 0,
                TransactionAccountName = dataRow[4]?.ToString() ?? string.Empty,
                TransferDate = dataRow[9].ParseToDateTime(),
                TransferDescription = dataRow[10]?.ToString() ?? string.Empty,
                TransferAmount = DecimalToInt(dataRow[11].ParseToDecimal()) ?? 0
            };            

            return model;
        }

        internal static decimal? ParseToDecimal(this object? obj)
        {
            if (obj == null)
            {
                return null;
            }
            var parsed = decimal.TryParse(obj.ToString(), out var parsedObj);

            return parsed ? parsedObj : null;
        }

        internal static DateTime? ParseToDateTime(this object? data)
        {
            if (data == null)
            { 
                return null; 
            }
            var parsed = DateTime.TryParse(data.ToString(), out var parsedData);

            return parsed ? parsedData : null;
        }

        private static int? DecimalToInt(decimal? value)
        {
            if(value == null)
            {
                return null;
            }
            var result = (int)(value * 100);
            return result;
        }

        public static bool IsNullOrZero(this int? num)
        {
            return num == null || num == 0;
        }
    }
}
