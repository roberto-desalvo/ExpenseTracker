using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Importer.Parsers.Abstractions
{
    public interface IExcelFileReader
    {
        DataSet ReadExcelFile(string filePath);
    }
}
