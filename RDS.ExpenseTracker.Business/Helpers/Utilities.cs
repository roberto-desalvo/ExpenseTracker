using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public static class Utilities
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
    }
}
