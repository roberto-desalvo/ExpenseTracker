using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Importer.Utilities
{
    public static class Utils
    {
        public static bool ContainsOne(this string str, params string[] compare)
        {
            return ContainsOne(str, false, compare);
        }

        public static bool ContainsOne(this string str, bool ignoreCase, params string[] compare)
        {
            foreach (var s in compare)
            {
                if (str.Contains(s, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture))
                {
                    return true;
                }
            }
            return false;
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

        public static int? DecimalToCubedInt(decimal? value)
        {
            if (value == null)
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
