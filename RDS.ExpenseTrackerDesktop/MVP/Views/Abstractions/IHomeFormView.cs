using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.MVP.Views.Abstractions
{
    public interface IHomeFormView
    {
        void UpdateContantiAvailability(decimal? availability);
        void UpdateHypeAvailability(decimal? availability);
        void UpdateSatispayAvailability(decimal? availability);
        void UpdateSellaAvailability(decimal? availability);
    }
}
