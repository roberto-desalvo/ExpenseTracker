using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public static class ExtensionMethods
    {
        public static bool ContainsOne(this string str, params string[] compare)
        {
            foreach (var s in compare)
            {
                if (str.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
