using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.MVP.Models
{
    public class AvailabilityViewModel
    {
        public FinancialAccountViewModel Sella { get; set; } 
        public FinancialAccountViewModel Contanti { get; set; } 
        public FinancialAccountViewModel Hype { get; set; } 
        public FinancialAccountViewModel Satispay { get; set; }

        public AvailabilityViewModel()
        {
            Sella = new FinancialAccountViewModel();
            Contanti = new FinancialAccountViewModel();
            Hype = new FinancialAccountViewModel();
            Satispay = new FinancialAccountViewModel();
        }
    }
}
