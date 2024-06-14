using RDS.ExpenseTracker.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.Models
{
    public class TransactionDataGridModel
    {
        public int Id { get; set; }
        public DateOnly? Date { get; set; }
        public int Amount { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
