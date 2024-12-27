using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.TransactionImport.Parsers.Models
{
    public class XlsDataRowModel
    {
        public DateTime? TransactionDate { get; set; }
        public string TransactionDescription { get; set; } = string.Empty;
        public int TransactionAmount { get; set; }
        public string TransactionAccountName { get; set; } = string.Empty;
        public DateTime? TransferDate { get; set; }
        public string TransferDescription { get; set; } = string.Empty;
        public int TransferAmount { get; set; }
    }
}
