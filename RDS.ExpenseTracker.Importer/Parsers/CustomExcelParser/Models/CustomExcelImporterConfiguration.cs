using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Models
{
    public class CustomExcelImporterConfiguration
    {
        public string FilePath { get; set; } = string.Empty;
        public IList<string> SheetsToIgnore { get; set; } = [];
        public Dictionary<string, int> AccountInitialAmounts { get; set; } = [];
        public string FirstAccountName { get; set; } = string.Empty;
        public string SecondAccountName { get; set; } = string.Empty;
        public string ThirdAccountName { get; set; } = string.Empty;
        public int TransactionDateIndex { get; set; }
        public int TransactionDescriptionIndex { get; set; }
        public int TransactionOutflowIndex { get; set; }
        public int TransactionInflowIndex { get; set; }
        public int TransactionAccountNameIndex { get; set; }
        public int TransferDateIndex { get; set; }
        public int TransferDescriptionIndex { get; set; }
        public int TransferAmountIndex { get; set; }
    }
}
