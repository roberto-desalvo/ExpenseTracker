using RDS.ExpenseTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.Models
{
    public class TransactionGridModel
    {
        public int Id { get; set; }
        public DateOnly? Date { get; set; }
        public int Amount { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
