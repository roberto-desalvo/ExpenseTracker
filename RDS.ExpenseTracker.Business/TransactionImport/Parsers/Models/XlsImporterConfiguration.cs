using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.TransactionImport.Parsers.Models
{
    public class XlsImporterConfiguration
    {
        public string FilePath { get; set; }
        public IList<string> SheetsToIgnore { get; set; }
        public Dictionary<string, int> AccountInitialAmounts { get; set; } = new Dictionary<string, int>();


        public XlsImporterConfiguration(string filePath, IList<string> sheetsToIgnore)
        {
            FilePath = filePath;
            SheetsToIgnore = sheetsToIgnore ?? new List<string>();
        }
    }
}
