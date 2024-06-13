using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.Models
{
    class MainViewModel
    {
        public int SellaAvailability { get; set; }
        public int SatispayAvailability { get; set; }
        public int ContantiAvailability { get; set; }
        public int HypeAvailability { get; set; }

        public ICollection<TransactionDataGridViewModel> DataGridRecords { get; set; }
    }
}
