using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.Models
{
    public class ExcelDataRowModel
    {
        public DateTime? TransactionDate { get; set; }
        public string TransactionDescription { get; set; } = string.Empty;
        public decimal TransactionAmount { get; set; }
        public string TransactionAccountName { get; set; } = string.Empty;
        public DateTime? TransferDate { get; set; }
        public string TransferDescription { get; set; } = string.Empty;
        public decimal TransferAmount { get; set; }
    }
}
