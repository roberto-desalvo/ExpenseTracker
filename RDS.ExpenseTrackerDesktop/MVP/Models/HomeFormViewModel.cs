using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.MVP.Models
{
    public class HomeFormViewModel
    {
        public AvailabilityViewModel Availabilities { get; set; }

        public HomeFormViewModel()
        {
            Availabilities = new AvailabilityViewModel();
        }
    }
}
