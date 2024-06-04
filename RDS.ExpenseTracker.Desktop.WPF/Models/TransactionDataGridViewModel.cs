using RDS.ExpenseTracker.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.Models
{
    public class TransactionDataGridViewModel
    {
        public int Id { get; set; }
        public DateOnly? Date { get; set; }
        public decimal Amount { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public CategoryEnum Category { get; set; }
    }
}
