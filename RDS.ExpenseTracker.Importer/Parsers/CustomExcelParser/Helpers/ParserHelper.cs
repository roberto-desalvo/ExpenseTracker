using RDS.ExpenseTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Helpers
{
    public static class ParserHelper
    {
        public static DateTime ParseDateFromSheetName(string name)
        {
            try
            {
                var index = name.IndexOf('2');
                var year = int.Parse(name[index..].Trim());
                var monthStr = name[..index].Trim().ToLower();

                var months = new CultureInfo("it-IT").DateTimeFormat.MonthNames.Select(x => x.ToLowerInvariant()).ToArray();
                var month = Array.IndexOf(months, monthStr) + 1;

                return new DateTime(year, month, 1);
            }
            catch (Exception)
            {
                throw new FormatException($"Invalid date format, please check date form sheet {name}");
            }
        }

        public static void CheckAndAssignDate(Transaction transaction, DateTime defaultDate)
        {
            var registeredDate = transaction.Date;
            transaction.Date = registeredDate == null ? defaultDate : new DateTime(defaultDate.Year, registeredDate.Value.Month, registeredDate.Value.Day);
        }
    }
}
