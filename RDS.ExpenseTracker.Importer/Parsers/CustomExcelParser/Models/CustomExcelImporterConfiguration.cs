using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Models
{
    public class CustomExcelImporterConfiguration
    {
        public string FilePath { get; set; }
        public IList<string> SheetsToIgnore { get; set; }
        public Dictionary<string, int> AccountInitialAmounts { get; set; } = new Dictionary<string, int>();


        public CustomExcelImporterConfiguration(string filePath, IList<string> sheetsToIgnore)
        {
            FilePath = filePath;
            SheetsToIgnore = sheetsToIgnore ?? new List<string>();
        }
    }
}
