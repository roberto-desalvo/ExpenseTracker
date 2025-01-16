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
            var index = name.IndexOf('2');
            var year = int.Parse(name[index..].Trim());
            var monthStr = name[..index].Trim().ToLower();

            var months = new CultureInfo("it-IT").DateTimeFormat.MonthNames.Select(x => x.ToLowerInvariant()).ToArray();
            var month = Array.IndexOf(months, monthStr) + 1;

            return new DateTime(year, month, 1);
        }
    }
}
