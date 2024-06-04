using RDS.ExpenseTracker.Desktop.MVP.Components;
using RDS.ExpenseTracker.Desktop.MVP.Presenters.Abstractions;
using RDS.ExpenseTracker.Desktop.MVP.Views.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.MVP.Views
{
    public class HomeFormView : IHomeFormView
    {
        private readonly HomeFormComponents _components;
        private readonly IHomeFormPresenter _presenter;
        public HomeFormView(HomeFormComponents components) 
        { 
            _components = components ?? throw new ArgumentNullException(nameof(components));
            //_presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }
        public void UpdateContantiAvailability(decimal? availability)
        {
            _components.ContantiLabel.Text = availability.ToString();
        }

        public void UpdateHypeAvailability(decimal? availability)
        {
            _components.HypeLabel.Text = availability.ToString();
        }

        public void UpdateSatispayAvailability(decimal? availability)
        {
            _components.SatispayLabel.Text = availability.ToString();
        }

        public void UpdateSellaAvailability(decimal? availability)
        {
            _components.SellaLabel.Text = availability.ToString();
        }
    }
}
