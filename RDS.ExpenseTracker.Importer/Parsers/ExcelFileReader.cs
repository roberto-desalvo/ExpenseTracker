using ExcelDataReader;
using RDS.ExpenseTracker.Importer.Parsers.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Importer.Parsers
{
    public class ExcelFileReader : IExcelFileReader
    {
        public DataSet ReadExcelFile(string filePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            return reader.AsDataSet();
        }
    }
}
