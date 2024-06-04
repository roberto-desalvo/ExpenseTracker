using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.MVP.Models;
using RDS.ExpenseTracker.Desktop.MVP.Presenters.Abstractions;
using RDS.ExpenseTracker.Desktop.MVP.Views.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.MVP.Presenters
{
    public class HomeFormPresenter : IHomeFormPresenter
    {
        private readonly HomeFormViewModel _model;
        private readonly IFinancialAccountService _accountService;
        private readonly IHomeFormView _view;

        public HomeFormPresenter(HomeFormViewModel model, IFinancialAccountService accountService, IHomeFormView view)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _view = view ?? throw new ArgumentNullException(nameof(view));

            Load();
        }

        private void Load()
        {
            var satispay = _accountService.GetFinancialAccount(1);
            var sella = _accountService.GetFinancialAccount(2);
            var contanti = _accountService.GetFinancialAccount(3);
            var hype = _accountService.GetFinancialAccount(4);

            _model.Availabilities.Satispay.FinancialAccount = satispay;
            _model.Availabilities.Sella.FinancialAccount = sella;
            _model.Availabilities.Contanti.FinancialAccount = contanti;
            _model.Availabilities.Hype.FinancialAccount = hype;

            _view.UpdateSellaAvailability(sella?.Availability);
            _view.UpdateSatispayAvailability(satispay?.Availability);
            _view.UpdateContantiAvailability(contanti?.Availability);
            _view.UpdateHypeAvailability(hype?.Availability);

        }
    }
}
