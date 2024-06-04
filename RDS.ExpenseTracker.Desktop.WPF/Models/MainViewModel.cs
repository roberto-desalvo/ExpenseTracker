using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.Models
{
    class MainViewModel
    {
        public decimal SellaAvailability { get; set; }
        public decimal SatispayAvailability { get; set; }
        public decimal ContantiAvailability { get; set; }
        public decimal HypeAvailability { get; set; }

        public ICollection<TransactionDataGridViewModel> DataGridRecords { get; set; }
    }
}
