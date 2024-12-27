using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.TransactionImport.Parsers.Models
{
    public static class XlsConstants
    {
        public const string SellaAccountName = "Sella";
        public const string HypeAccountName = "Hype";
        public const string SatispayAccountName = "Satispay";
        public const int TransactionDateIndex = 0;
        public const int TransactionDescriptionIndex = 1;
        public const int TransactionOutflowIndex = 2;
        public const int TransactionInflowIndex = 3;
        public const int TransactionAccountNameIndex = 4;
        public const int TransferDateIndex = 9;
        public const int TransferDescriptionIndex = 10;
        public const int TransferAmountIndex = 11;
    }
}
